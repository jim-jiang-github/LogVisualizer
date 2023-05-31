using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;
using LogVisualizer.Scenarios.Schemas.Logs;

namespace LogVisualizer.Scenarios.Sources
{
    public class LogSourceLoader
    {
        public static ILogSource? Load(string logFilePath, string schemaLogPath)
        {
            var schemaType = Schema.GetSchemaTypeFromJsonFile(schemaLogPath);
            switch (schemaType)
            {
                case SchemaType.LogText:
                    var logSourceText = LoadLogSource<LogSourceText, SchemaLogText>(logFilePath, schemaLogPath);
                    return logSourceText;
                case SchemaType.LogBinary:
                    var logSourceBinary = LoadLogSource<LogSourceBinary, SchemaLogBinary>(logFilePath, schemaLogPath);
                    return logSourceBinary;
            }
            return null;
        }
        private static Stream? LoadLogFileStream(string logFilePath, SchemaLog schemaLog)
        {
            var extension = Path.GetExtension(logFilePath);
            var available = schemaLog.SupportedExtensions.Contains(extension);
            if (!available)
            {
                Log.Error("SchemaLog {AvailableExtensions} not available", string.Join(",", schemaLog.SupportedExtensions));
                return null;
            }
            var streamLoader = LogLoaderProvider.GetLoader(schemaLog.LoaderType);
            if (streamLoader == null)
            {
                Log.Error("{File} can not find stream loader {Type}", logFilePath, schemaLog.LoaderType);
                return null;
            }
            var stream = streamLoader.Load(logFilePath);
            return stream;
        }
        private static ILogSource? LoadLogSource<TLogSource, TSchemaLog>(string logFilePath, string schemaLogPath)
            where TLogSource : class, ILogSource
            where TSchemaLog : SchemaLog, new()
        {
            var schemaLog = IJsonSerializable.LoadFromJsonFile<TSchemaLog>(schemaLogPath);
            if (schemaLog == null)
            {
                return null;
            }
            var stream = LoadLogFileStream(logFilePath, schemaLog);
            if (stream == null)
            {
                return null;
            }
            try
            {
                var logSourceConstructors = (typeof(TLogSource)).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                var logSource = logSourceConstructors[0].Invoke(new object[] { stream, schemaLog }) as TLogSource;
                return logSource;
            }
            catch (Exception ex)
            {
                Log.Error("Load {LogSource} error {Error}", typeof(TLogSource), ex);
                return null;
            }
        }
    }
}
