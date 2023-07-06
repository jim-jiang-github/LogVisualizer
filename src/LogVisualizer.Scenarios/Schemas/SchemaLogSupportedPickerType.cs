using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas
{
    public class SchemaLogSupportedPickerType
    {
        public string SupportedExtension { get; set; } = "txt";
        public string? FileNameValidateRegex { get; set; } = null;

        public string GetConvertedResult(string origin)
        {
            return origin;
        }
    }
}
