using LogVisualizer.Decompress;
using LogVisualizer.Scenarios.Contents;
using LogVisualizer.Scenarios.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LogVisualizer.Scenarios.Scenario;

namespace LogVisualizer.Scenarios
{
    public class Scenario
    {
        private const string SCHEMA_FOLDER_NAME = "Schemas";
        private const string SCHEMA_LOG_NAME = "schema_log.json";

        public static Scenario? LoadFromFolder(string folder)
        {
            string name;
            if (folder.EndsWith('\\'))
            {
                name = Path.GetFileName(Path.GetDirectoryName(folder));
            }
            else
            {
                name = Path.GetFileName(folder);
            }
            var schemaLogPath = Path.Combine(folder, SCHEMA_FOLDER_NAME, SCHEMA_LOG_NAME);
            var schemaLogLoader = SchemaLogLoader.GetSchemalogReaderFromJsonFile(schemaLogPath);
            if (schemaLogLoader == null)
            {
                return null;
            }
            var scenario = new Scenario(schemaLogLoader, name, schemaLogPath);
            return scenario;
        }

        private SchemaLogLoader _schemaLogLoader;
        private string _schemaLogPath;

        public string[] SupportedExtensions { get; } = Array.Empty<string>();
        public string Name { get; }

        private Scenario(SchemaLogLoader schemaLogLoader, string name, string schemaLogPath)
        {
            _schemaLogLoader = schemaLogLoader;
            Name = name;
            _schemaLogPath = schemaLogPath;
            SupportedExtensions = schemaLogLoader.SupportedLoadTypes.Select(x => $"*.{x.SupportedExtension}").ToArray();
        }

        public ILogContent? LoadLogContent(string logSourcePath)
        {
            Stream? stream;
            if (ArchiveLoader.IsSupportedArchive(logSourcePath))
            {
                return null;
            }
            if (ArchiveLoader.IsArchiveEntry(logSourcePath))
            {
                stream = ArchiveReader.ReadStream(logSourcePath);
                if (stream == null)
                {
                    return null;
                }
            }
            else
            {
                var reader = LogReader.GetReader(LogReaderType.Text);
                if (reader == null)
                {
                    return null;
                }
                stream = reader.Read(logSourcePath);
                if (stream == null)
                {
                    return null;
                }
            }
            var logContent = ILogContent.LoadLogContent(stream, _schemaLogPath);
            return logContent;
        }
    }
}
