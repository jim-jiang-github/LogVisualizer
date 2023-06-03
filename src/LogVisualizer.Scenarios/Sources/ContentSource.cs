using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Sources
{
    internal class ContentSource
    {
        public string[] ColumnHeadTemplate { get; }
        public LogRow[] Rows { get; }
        public int RowCount { get; }
        public ContentSource(string[] columnHeadTemplate, LogRow[] rows, int rowCount)
        {
            ColumnHeadTemplate = columnHeadTemplate;
            Rows = rows;
            RowCount = rowCount;
        }

        public IEnumerable<LogRow> GetRows(int start, int length)
        {
            var rows = Rows.Skip(start).Take(length);
            return rows;
        }

        public LogRow? GetRow(int index)
        {
            if (index < 0)
            {
                return null;
            }
            if (index >= RowCount)
            {
                return null;
            }
            return Rows[index];
        }
    }
}
