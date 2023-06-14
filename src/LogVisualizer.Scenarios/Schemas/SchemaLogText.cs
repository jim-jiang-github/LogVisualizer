using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static LogVisualizer.Scenarios.Schemas.SchemaLogLoader;

namespace LogVisualizer.Scenarios.Schemas
{
    internal class SchemaLogText :
        SchemaLog<SchemaLogText.SchemaBlockText,
            SchemaLogText.SchemaColumnHeadText,
            SchemaLogText.SchemaCellText>
    {
        #region Internal Class
        public class SchemaBlockText : SchemaBlock
        {
            public string RegexStart { get; set; } = string.Empty;
            public string RegexEnd { get; set; } = string.Empty;
            public string RegexContent { get; set; } = string.Empty;
        }
        public class SchemaColumnHeadText : SchemaColumnHead
        {
            public string RegexStart { get; set; } = string.Empty;
            public string RegexContent { get; set; } = string.Empty;
        }
        public class SchemaCellText : SchemaCell
        {
            public int Index { get; set; }
        }
        #endregion
        public override SchemaLogType Type => SchemaLogType.LogText;
        public SchemaLogText()
        {
        }
        public string Default_22_2_20()
        {
            var timeConvertor = new SchemaConvertor()
            {
                Name = "Time",
                Type = ConvertorType.Time2Time,
                Expression = "[MM/dd/yy HH:mm:ss.fff][yyyy-MM-dd HH:mm:ss,fff]"
            };
            Convertors = new[] {
                timeConvertor
            };
            Loader = new SchemaLogLoader()
            {
                SupportedLoadTypes = new[]
                {
                    new SchemaLogSupportedLoadType()
                    {
                        SupportedExtension = "log",
                        FileNameValidateRegex = @"RoomsHost-\d+_\d+_pid-\d+(\.\S+|)$",
                        ReaderType = LogReaderType.Text
                    },
                }
            };
            ColumnHeadTemplate = new SchemaColumnHeadText
            {
                RegexStart = @"^(\d{2}\/\d{2}\/\d{2} \d{2}:\d{2}:\d{2}.\d{3})",
                RegexContent = @"^(\d{2}\/\d{2}\/\d{2} \d{2}:\d{2}:\d{2}.\d{3}) \<(.*?)\> \[(.*?)\] (.*?) (.*)",
                Columns = new SchemaColumn[]
                {
                    new SchemaColumn
                    {
                        Cell = new SchemaCellText{Name = "Time", Index = 1, ConvertorName = "Time"},
                    },
                    new SchemaColumn
                    {
                        Cell = new SchemaCellText{Name = "Module", Index = 2},
                        Filterable=true
                    },
                    new SchemaColumn
                    {
                        Cell = new SchemaCellText{Name = "Thread", Index = 3},
                        Filterable=true
                    },
                    new SchemaColumn
                    {
                        Cell = new SchemaCellText{Name = "Level", Index = 4},
                        Filterable=true
                    },
                    new SchemaColumn
                    {
                        Cell = new SchemaCellText{Name = "Msg", Index = 5},
                        Enumeratable=true
                    }
                }
            };
            return this.SaveAsJson($"schema_log.json");
        }
        public string Default_21_4_30()
        {
            var header = new SchemaBlockText()
            {
                Name = "Header",
                RegexStart = @"app: (RoomsController)/(.*?)/(SHA\(.*\))",
                RegexEnd = @"os: (.*?)/(.*?)[\r|\n]",
                RegexContent = @"app: (RoomsController)/(.*?)/(SHA\(.*\))[\r|\n]*os: (.*?)/(.*?)[\r|\n]",
                Cells = new SchemaCellText[]
                {
                    new SchemaCellText{Name = "Type", Index=1},
                    new SchemaCellText{Name = "Version", Index=2},
                    new SchemaCellText{Name = "EncryptKey", Index=3},
                    new SchemaCellText{Name = "OS", Index=4},
                    new SchemaCellText{Name = "OSVersion", Index=5},
                }
            };
            Blocks.Add(header);
            Loader = new SchemaLogLoader()
            {
                SupportedLoadTypes = new[]
                {
                    new SchemaLogSupportedLoadType()
                    {
                        SupportedExtension = "log",
                        FileNameValidateRegex = @"RoomsHost-\d+_\d+_pid-\d+(\.\S+|)$",
                        ReaderType = LogReaderType.Text
                    },
                }
            };

            ColumnHeadTemplate = new SchemaColumnHeadText
            {
                RegexStart = @"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3} [A-Z]{3})",
                RegexContent = @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3} [A-Z]{3}) : (.*?) : \[(.*?)\] \[(.*?)\] (.*)",
                Columns = new SchemaColumn[]
                {
                    new SchemaColumn
                    {
                        Cell=new SchemaCellText{Name = "Time", Index=1, ConvertorName="Time"},
                    },
                    new SchemaColumn
                    {
                        Cell=new SchemaCellText{Name = "Module", Index=2},
                        Filterable=true
                    },
                    new SchemaColumn
                    {
                        Cell=new SchemaCellText{Name = "Thread", Index=3},
                        Filterable=true
                    },
                    new SchemaColumn
                    {
                        Cell=new SchemaCellText{Name = "Level", Index=4},
                        Filterable=true
                    },
                    new SchemaColumn
                    {
                        Cell=new SchemaCellText{Name = "Msg", Index=5},
                        Enumeratable=true
                    }
                }
            };
            return this.SaveAsJson($"schema_log.json");
        }
    }
}
