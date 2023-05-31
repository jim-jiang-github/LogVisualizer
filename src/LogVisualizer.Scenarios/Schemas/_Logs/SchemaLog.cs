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
    internal abstract class SchemaLog : Schema
    {
        public string Name { get; set; } = string.Empty;
        public string EncodingName { get; set; } = "utf-8";
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLoaderType LoaderType { get; set; } = LogLoaderType.Unknow;
        public string[] SupportedExtensions { get; set; } = Array.Empty<string>();
        public List<SchemaConvertor> Convertors { get; } = new List<SchemaConvertor>();

        public static string[] GetSupportedExtensionsFromJsonContent(string jsonContent)
        {
            var anonymousType = new { SupportedExtensions = Array.Empty<string>() };
            var result = GetAnonymousTypeFromJsonContent(anonymousType, (c) =>
            {
                var x = JsonConvert.DeserializeAnonymousType(c, anonymousType);
                if (x == null)
                {
                    return Array.Empty<string>();
                }
                return x.SupportedExtensions;
            }, jsonContent);
            if (result == null)
            {
                return Array.Empty<string>();
            }
            return result;
        }
        public static LogLoaderType GetLogFileLoaderTypeFromJsonContent(string jsonContent)
        {
            var anonymousType = new { LogFileLoaderType = LogLoaderType.Unknow };
            var result = GetAnonymousTypeFromJsonContent(anonymousType, (c) =>
            {
                var x = JsonConvert.DeserializeAnonymousType(c, anonymousType);
                if (x == null)
                {
                    return LogLoaderType.Unknow;
                }
                return x.LogFileLoaderType;
            }, jsonContent);
            return result;
        }
    }
    /// <summary>
    /// SchemaLog|
    ///          |Block1|     
    ///          |      |Cell1,Cell2...CellN
    ///          |Block2|     
    ///          .      |Cell1,Cell2...CellN
    ///          .
    ///          . 
    ///          |BlockN|     
    ///                 |Cell1,Cell2...CellN
    ///          |ColumnHeadTemplate|     
    ///                 |Cell1,Cell2...CellN
    /// </summary>
    /// <typeparam name="TBlockSchema"></typeparam>
    /// <typeparam name="TColumnHeadSchema"></typeparam>
    /// <typeparam name="TCellSchema"></typeparam>
    internal abstract class SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema> : SchemaLog
        where TBlockSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaBlock, new()
        where TColumnHeadSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaColumnHead, new()
        where TCellSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaCell, new()
    {
        #region Internal Class
        public class SchemaBlock
        {
            public string Name { get; set; } = string.Empty;
            public TCellSchema[] Cells { get; set; } = Array.Empty<TCellSchema>();
        }
        public class SchemaColumnHead
        {
            public SchemaColumn[] Columns { get; set; } = Array.Empty<SchemaColumn>();
        }
        public class SchemaColumn
        {
#nullable disable
            public TCellSchema Cell { get; set; }
#nullable enable
            public bool Enumeratable { get; set; } = false;
            public bool Filterable { get; set; } = false;
        }
        public class SchemaCell
        {
            public string Name { get; set; } = string.Empty;
            public string? ConvertorName { get; set; }
        }
        #endregion
        public List<TBlockSchema> Blocks { get; } = new List<TBlockSchema>();
        public TColumnHeadSchema ColumnHeadTemplate { get; set; } = new TColumnHeadSchema();
    }
}
