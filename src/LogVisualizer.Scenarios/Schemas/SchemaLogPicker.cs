using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas
{
    internal class SchemaLogPicker
    {
        public static SchemaLogPicker? GetSchemaLogPickerFromJsonFile(string jsonFilePath)
        {
            var schemaLogContent = IJsonSerializable.LoadContentFromJsonFile(jsonFilePath);
            if (schemaLogContent == null)
            {
                return null;
            }
            var anonymousType = new { Picker = new SchemaLogPicker() };
            var type = JsonConvert.DeserializeAnonymousType(schemaLogContent, anonymousType)?.Picker ?? null;
            return type;
        }

        public SchemaLogSupportedPickerType[] SupportedPickerTypes { get; set; } = Array.Empty<SchemaLogSupportedPickerType>();
    }
}
