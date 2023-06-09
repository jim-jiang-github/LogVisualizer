using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas
{
    internal class SchemalogReader
    {
        public class SchemaLogLoadStep
        {
            public string SupportedExtension { get; set; } = "txt";
            public string[] FileNameValidateRegexs { get; set; } = Array.Empty<string>();
            [JsonConverter(typeof(StringEnumConverter))]
            public LogReaderType ReaderType { get; set; } = LogReaderType.Text;
        }

        public static SchemalogReader? GetSchemalogReaderFromJsonFile(string jsonFilePath)
        {
            var schemaLogContent = IJsonSerializable.LoadContentFromJsonFile(jsonFilePath);
            if (schemaLogContent == null)
            {
                return null;
            }
            var anonymousType = new { Loader = new SchemalogReader() };
            var type = JsonConvert.DeserializeAnonymousType(schemaLogContent, anonymousType)?.Loader ?? null;
            return type;
        }

        public SchemaLogLoadStep[] LoadSteps { get; set; } = Array.Empty<SchemaLogLoadStep>();
    }
}
