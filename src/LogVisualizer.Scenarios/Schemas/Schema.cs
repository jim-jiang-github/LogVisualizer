using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas
{
    internal abstract class Schema : IJsonSerializable
    {
        public static SchemaLogType GetSchemaTypeFromJsonFile(string jsonFilePath)
        {
            var schemaLogContent = IJsonSerializable.LoadContentFromJsonFile(jsonFilePath);
            if (schemaLogContent == null)
            {
                return SchemaLogType.Unknow;
            }
            var anonymousType = new { Type = SchemaLogType.Unknow };
            var type = JsonConvert.DeserializeAnonymousType(schemaLogContent, anonymousType)?.Type ?? SchemaLogType.Unknow;
            return type;
        }
    }
}
