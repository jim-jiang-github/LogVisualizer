using Microsoft.CodeAnalysis.FlowAnalysis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;
using System.Reflection.PortableExecutable;

namespace LogVisualizer.Scenarios.Sources
{
    internal class LogSourceText : LogSource<SchemaLogText.SchemaBlockText, SchemaLogText.SchemaColumnHeadText, SchemaLogText.SchemaCellText>
    {
        private readonly StreamReader _streamReader;

        private LogSourceText(Stream stream, SchemaLog<SchemaLogText.SchemaBlockText, SchemaLogText.SchemaColumnHeadText, SchemaLogText.SchemaCellText> schemaLog) : base(stream, schemaLog)
        {
            _streamReader = new StreamReader(stream, _encoding);
        }

        protected override BlockSource CreateBlockSource(SchemaLogText.SchemaBlockText block)
        {
            string? line = _streamReader.ReadLine();
            while (line != null && !Regex.IsMatch(line, block.RegexStart, RegexOptions.Singleline))
            {
                line = _streamReader.ReadLine();
            }
            if (line == null)
            {
                throw new ArgumentException("block format error.");
            }
            var stringBuilder = new StringBuilder(line);
            line = _streamReader.ReadLine();
            while (line != null && !Regex.IsMatch(line, block.RegexEnd, RegexOptions.Singleline))
            {
                stringBuilder.AppendLine(line);
                line = _streamReader.ReadLine();
            }
            stringBuilder.AppendLine(line);
            var itemContent = stringBuilder.ToString();
            var match = Regex.Match(itemContent, block.RegexContent, RegexOptions.Singleline);
            if (match.Groups.Count != block.Cells.Length + 1)
            {
                throw new ArgumentException("block cell format error.");
            }
            var blockCells = new BlockCellSource[block.Cells.Length];
            for (int i = 0; i < block.Cells.Length; i++)
            {
                var cell = block.Cells[i];
                var cellConvertor = _convertorProvider.GetConvertor(cell.ConvertorName);
                var captureCell = match.Groups[cell.Index].Value;
                blockCells[i] = new BlockCellSource(cell.Name, cellConvertor?.Convert(captureCell) ?? "");
            }
            return new BlockSource(block.Name, blockCells);
        }

        private IEnumerable<string> LogLines()
        {
            string previousLine = string.Empty;
            string? currentLine;

            while ((currentLine = _streamReader.ReadLine()) != null)
            {
                if (Regex.IsMatch(currentLine, _schemaLog.ColumnHeadTemplate.RegexStart))
                {
                    if (!string.IsNullOrEmpty(previousLine))
                    {
                        yield return previousLine;
                    }
                    previousLine = currentLine;
                }
                else
                {
                    previousLine += Environment.NewLine + currentLine;
                }
            }

            if (!string.IsNullOrEmpty(previousLine))
            {
                yield return previousLine;
            }
        }

        protected override ContentSource CreateContentSource(
            ICellFinder cellFinder)
        {

            var cellConvertors = new CellConvertor?[_schemaLog.ColumnHeadTemplate.Columns.Length];
            for (int i = 0; i < _schemaLog.ColumnHeadTemplate.Columns.Length; i++)
            {
                cellConvertors[i] = _convertorProvider.GetConvertor(_schemaLog.ColumnHeadTemplate.Columns[i].Cell.ConvertorName);
            }

            var rowSources = LogLines()
                .AsParallel()
                .AsOrdered()
                .Select((l, i) =>
                {
                    var match = Regex.Match(l, _schemaLog.ColumnHeadTemplate.RegexContent, RegexOptions.Singleline);
                    var captureCells = match.Groups.Values.Skip(1);
                    var cells = cellConvertors.Zip(captureCells, (convertor, cell) =>
                    {
                        if (convertor == null)
                        {
                            return cell.Value;
                        }
                        else
                        {
                            return convertor.Convert(cell.Value);
                        }
                    });
                    return new LogRow(i, cells.ToArray());
                }).ToArray();

            return new ContentSource(_schemaLog.ColumnHeadTemplate.Columns.Select(t => t.Cell.Name).ToArray(), rowSources, rowSources.Length);
        }

        public override void Dispose()
        {
            base.Dispose();
            _streamReader?.Dispose();
        }
    }
}
