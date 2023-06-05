using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;
using LogVisualizer.Scenarios.Contents;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace LogVisualizer.Scenarios
{
    /// <summary>
    /// This must use public, Otherwise '_script.RunAsync(_parameter).Result' will cause 'is inaccessible due to its protection level' error.
    /// 
    /// The parameter of CSharpScript expression
    /// 
    /// var script = CSharpScript.Create<int>("Value*Value", globalsType: typeof(CSharpScriptGlobalParameter<int>));
    /// script.Compile();
    /// var qweqwe = script.RunAsync(new CSharpScriptGlobalParameter<int>()
    /// {
    ///     Value = 11111
    /// }).Result.ReturnValue;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CSharpScriptGlobalParameter<T>
    {
        public T? Value { get; set; }
    }
    internal class CellConvertorProvider
    {
        #region InnerClass
        public enum ConvertorType
        {
            Math,
            Long2Time,
            Time2Time,
            Enum,
        }
        internal abstract class CellConvertor
        {
            internal const string CELL_VALUE = "CellValue";
            protected string Expression { get; set; }
            public CellConvertor? ContinueConvertor { get; set; }
            protected CellConvertor(ICellFinder cellFinder, string expression)
            {
                Expression = expression;
                var pattern = @"{(.*?)}";
                var matches = Regex.Matches(Expression, pattern);
                var regex = new Regex(pattern);
                foreach (Match match in matches)
                {
                    if (match.Success && match.Groups.Count >= 1 &&
                        match.Groups[1].Value != CELL_VALUE &&
                        cellFinder.GetCellValue(match.Groups[1].Value) is string replacement)
                    {
                        Expression = regex.Replace(Expression, replacement, 1);
                    }
                }
            }
            public object? Convert(object? value)
            {
                value = ConvertInternal(value);
                if (ContinueConvertor != null)
                {
                    value = ContinueConvertor.Convert(value);
                }
                return value;
            }
            protected abstract object? ConvertInternal(object? value);
        }
        private class CellConvertorEnum : CellConvertor
        {
            private readonly Dictionary<int, string> _enumDictionary;
            public CellConvertorEnum(ICellFinder cellFinder, string expression) : base(cellFinder, expression)
            {
                var matches = Regex.Matches(expression, @"(\d:[A-Z|a-z]*)");
                if (!matches.Any(m => m.Success))
                {
                    _enumDictionary = new Dictionary<int, string>();
                    return;
                }
                _enumDictionary = matches.Select(m => m.Value.Split(':')).
                    Where(x => x.Length == 2).
                    Select(x => new
                    {
                        Key = int.TryParse(x[0], out int key) ? (int?)key : null,
                        Value = x[1]
                    }).
                    Where(x => x.Key != null).
                    ToDictionary(x => x.Key ?? -1, x => x.Value);
            }
            protected override object? ConvertInternal(object? value)
            {
                var input = value?.ToString();
                if (input == null)
                {
                    return value;
                }
                if (int.TryParse(input, out int enumValue) && _enumDictionary.TryGetValue(enumValue, out string? enumString))
                {
                    return enumString;
                }
                return value;
            }
        }
        private class CellConvertorLong2Time : CellConvertor
        {
            public CellConvertorLong2Time(ICellFinder cellFinder, string expression) : base(cellFinder, expression)
            {
            }
            protected override object? ConvertInternal(object? value)
            {
                var input = value?.ToString();
                if (input == null)
                {
                    return value;
                }
                if (long.TryParse(input, out long milliseconds))
                {
                    var timeSpant = TimeSpan.FromMilliseconds(milliseconds);
                    var dateTime = DateTime.UnixEpoch + timeSpant + TimeZoneInfo.Local.BaseUtcOffset;
                    var time = dateTime.ToString(Expression);
                    return time;
                }
                return value;
            }
        }
        private class CellConvertorMath : CellConvertor
        {
            private readonly CSharpScriptGlobalParameter<long> _parameter = new CSharpScriptGlobalParameter<long>();
            private ScriptRunner<long>? _runner;

            public CellConvertorMath(ICellFinder cellFinder, string expression) : base(cellFinder, expression)
            {
                var pattern = @"{" + CELL_VALUE + "}";
                Expression = Regex.Replace(Expression, pattern, nameof(CSharpScriptGlobalParameter<long>.Value));
                ScriptOptions.Default.WithEmitDebugInformation(false);
                var script = CSharpScript.Create<long>(Expression, globalsType: typeof(CSharpScriptGlobalParameter<long>));
                try
                {
                    _runner = script.CreateDelegate();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "CellConvertorMath CSharpScript error");
                    _runner = null;
                }
            }

            protected override object? ConvertInternal(object? value)
            {
                var input = value?.ToString();
                if (input == null)
                {
                    return value;
                }
                if (_runner == null)
                {
                    return value;
                }
                if (int.TryParse(input, out int tickOffset))
                {
                    _parameter.Value = tickOffset;
                    var result = _runner.Invoke(_parameter).Result;
                    return result;
                }
                return value;
            }
        }
        private class CellConvertorTime2Time : CellConvertor
        {
            public CellConvertorTime2Time(ICellFinder cellFinder, string expression) : base(cellFinder, expression)
            {
            }
            protected override object? ConvertInternal(object? value)
            {
                var input = value?.ToString();
                if (input == null)
                {
                    return value;
                }
                var matches = Regex.Matches(Expression, @"(?<=\[).*?(?=\])");
                if (matches.Count == 2)
                {
                    var fromTimeFormat = matches[0].Value.ToString();
                    var toTimeFormat = matches[1].Value.ToString();
                    if (DateTime.TryParseExact(input, fromTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetime))
                    {
                        var formatedTime = datetime.ToString(toTimeFormat);
                        return formatedTime;
                    }
                }
                return value;
            }
        }
        #endregion
        private readonly Dictionary<string, CellConvertor?> _convertorMap;

        public CellConvertorProvider(ICellFinder cellFinder, IEnumerable<SchemaConvertor> convertors)
        {
            _convertorMap = convertors.ToDictionary(x => x.Name, x => CreateConvertor(cellFinder, x));
        }

        private CellConvertor? CreateConvertor(ICellFinder cellFinder, SchemaConvertor? schemaConvertor)
        {
            if (schemaConvertor == null)
            {
                return null;
            }
            if (schemaConvertor.Expression == null)
            {
                return null;
            }
            CellConvertor? streamCellConvertor = schemaConvertor.Type switch
            {
                ConvertorType.Math => new CellConvertorMath(cellFinder, schemaConvertor.Expression),
                ConvertorType.Long2Time => new CellConvertorLong2Time(cellFinder, schemaConvertor.Expression),
                ConvertorType.Time2Time => new CellConvertorTime2Time(cellFinder, schemaConvertor.Expression),
                ConvertorType.Enum => new CellConvertorEnum(cellFinder, schemaConvertor.Expression),
                _ => null,
            };
            if (streamCellConvertor != null && schemaConvertor.ContinueWith != null)
            {
                streamCellConvertor.ContinueConvertor = CreateConvertor(cellFinder, schemaConvertor.ContinueWith);
            }
            return streamCellConvertor;
        }

        public CellConvertor? GetConvertor(string? convertorName)
        {
            if (_convertorMap == null)
            {
                return null;
            }
            if (convertorName == null)
            {
                return null;
            }
            if (_convertorMap.TryGetValue(convertorName, out CellConvertor? cellConvertor))
            {
                return cellConvertor;
            }
            return null;
        }
    }
}
