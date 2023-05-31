﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Schemas.Logs
{
    internal class SchemaConvertor
    {
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(StringEnumConverter))]
        public SchemaConvertorType Type { get; set; }
        public string? Expression { get; set; }
        public SchemaConvertor? ContinueWith { get; set; }
    }
}
