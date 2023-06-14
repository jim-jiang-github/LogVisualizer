using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas
{
    internal class SchemaLogLoader
    {
        public class SchemaLogSupportedLoadType
        {
            public string SupportedExtension { get; set; } = "txt";
            public string? FileNameValidateRegex = null;
            [JsonConverter(typeof(StringEnumConverter))]
            public LogReaderType ReaderType { get; set; } = LogReaderType.Text;
        }

        public static SchemaLogLoader? GetSchemalogReaderFromJsonFile(string jsonFilePath)
        {
            var schemaLogContent = IJsonSerializable.LoadContentFromJsonFile(jsonFilePath);
            if (schemaLogContent == null)
            {
                return null;
            }
            var anonymousType = new { Loader = new SchemaLogLoader() };
            var type = JsonConvert.DeserializeAnonymousType(schemaLogContent, anonymousType)?.Loader ?? null;
            return type;
        }

        public SchemaLogSupportedLoadType[] SupportedLoadTypes { get; set; } = Array.Empty<SchemaLogSupportedLoadType>();
    }
}
