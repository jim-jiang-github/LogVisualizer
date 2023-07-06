using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas
{
    public class SchemaLogSupportedLoadType
    {
        public string SupportedExtension { get; set; } = "txt";
        public string? FileNameValidateRegex { get; set; } = null;
        [JsonConverter(typeof(StringEnumConverter))]
        public LogReaderType ReaderType { get; set; } = LogReaderType.Text;
    }
}
