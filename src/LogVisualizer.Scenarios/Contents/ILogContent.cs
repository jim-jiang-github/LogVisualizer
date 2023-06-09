using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;
using System.Reflection;
using System.Collections;

namespace LogVisualizer.Scenarios.Contents
{
    public interface ILogContent : IDisposable
    {
        #region Provider

        public static ILogContent? LoadLogContent(Stream logFileStream, string schemaLogPath)
        {
            var schemaType = Schema.GetSchemaTypeFromJsonFile(schemaLogPath);
            switch (schemaType)
            {
                case SchemaLogType.LogText:
                    var logContentText = LoadLogContent<LogContentText, SchemaLogText>(logFileStream, schemaLogPath);
                    return logContentText;
                case SchemaLogType.LogBinary:
                    var logContentBinary = LoadLogContent<LogContentBinary, SchemaLogBinary>(logFileStream, schemaLogPath);
                    return logContentBinary;
            }
            return null;
        }
        private static ILogContent? LoadLogContent<TLogContent, TSchemaLog>(Stream logFileStream, string schemaLogPath)
           where TLogContent : class, ILogContent
           where TSchemaLog : SchemaLog, new()
        {
            var schemaLog = IJsonSerializable.LoadFromJsonFile<TSchemaLog>(schemaLogPath);
            if (schemaLog == null)
            {
                Log.Warning("Can not load schemaLog from {schemaLogPath}", schemaLogPath);
                return null;
            }
            try
            {
                var logContentType = typeof(TLogContent);
                var logContentConstructors = logContentType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                var logContent = logContentConstructors[0].Invoke(new object[] { logFileStream, schemaLog }) as TLogContent;
                var init = logContentType.GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);

                if (init != null)
                {
                    init.Invoke(logContent, null);
                }

                return logContent;
            }
            catch (Exception ex)
            {
                Log.Warning("Load {LogContent} error {Error}", typeof(TLogContent), ex);
                return null;
            }
        }

        #endregion

        string[] ColumnNames { get; }
        LogRow[] Rows { get; }
        int RowsCount { get; }
        IEnumerable<string> EnumerateWords { get; }
        LogFilter Filter { get; }
    }
}
