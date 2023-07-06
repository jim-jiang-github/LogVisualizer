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
using static LogVisualizer.Scenarios.Schemas.SchemaLogLoader;

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
            if (name == null)
            {
                return null;
            }
            var schemaLogPath = Path.Combine(folder, SCHEMA_FOLDER_NAME, SCHEMA_LOG_NAME);
            var schemaLogPicker = SchemaLogPicker.GetSchemaLogPickerFromJsonFile(schemaLogPath);
            if (schemaLogPicker == null)
            {
                return null;
            }
            var schemaLogLoader = SchemaLogLoader.GetSchemaLogLoaderFromJsonFile(schemaLogPath);
            if (schemaLogLoader == null)
            {
                return null;
            }
            var scenario = new Scenario(schemaLogPicker, schemaLogLoader, name, schemaLogPath);
            return scenario;
        }

        private SchemaLogPicker _schemaLogPicker;
        private SchemaLogLoader _schemaLogLoader;
        private string _schemaLogPath;

        public SchemaLogSupportedLoadType[] SupportedLoadTypes { get; } = Array.Empty<SchemaLogSupportedLoadType>();
        public string Name { get; }

        private Scenario(SchemaLogPicker schemaLogPicker, SchemaLogLoader schemaLogLoader, string name, string schemaLogPath)
        {
            _schemaLogPicker = schemaLogPicker;
            _schemaLogLoader = schemaLogLoader;
            Name = name;
            _schemaLogPath = schemaLogPath;
            SupportedLoadTypes = schemaLogLoader.SupportedLoadTypes;
        }

        public string? GetConvertedName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var supportedPickerType = _schemaLogPicker.SupportedPickerTypes
                .Where(x => $".{x.SupportedExtension}" == extension)
                .Where(x => x.FileNameValidateRegex == null || Regex.IsMatch(fileName, x.FileNameValidateRegex))
                .FirstOrDefault();
            return supportedPickerType?.GetConvertedResult(fileName);
        }

        public ILogContent? LoadLogContent(string logSourcePath)
        {
            var extension = Path.GetExtension(logSourcePath);
            var fileName = Path.GetFileName(logSourcePath);
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
                var reader = SupportedLoadTypes
                    .Where(x => $".{x.SupportedExtension}" == extension)
                    .Where(x => x.FileNameValidateRegex == null || Regex.IsMatch(fileName, x.FileNameValidateRegex))
                    .Select(x => LogReader.GetReader(x.ReaderType))
                    .FirstOrDefault();
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
