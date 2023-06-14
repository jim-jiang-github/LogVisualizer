using LogVisualizer.Scenarios.Schemas;
using System;
using System.Collections.Generic;
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
            var loader = SchemaLogLoader.GetSchemalogReaderFromJsonFile(schemaLogPath);
            if (loader == null)
            {
                return null;
            }
            var supportedExtension = loader.SupportedLoadTypes.Select(x => $"*.{x.SupportedExtension}").ToArray();
            var scenario = new Scenario(name, schemaLogPath, supportedExtension);
            return scenario;
        }

        public string[] SupportedExtensions { get; } = Array.Empty<string>();
        public string Name { get; }
        public string SchemaLogPath { get; }

        private Scenario(string name, string schemaLogPath, string[] supportedExtensions)
        {
            Name = name;
            SchemaLogPath = schemaLogPath;
            SupportedExtensions = supportedExtensions;
        }
    }
}
