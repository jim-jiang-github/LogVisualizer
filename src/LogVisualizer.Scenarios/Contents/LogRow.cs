using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Contents
{
    public struct LogRow
    {
        public int Index { get; }
        public object?[] Cells { get; }

        public object? this[int index]
        {
            get
            {
                return Cells[index];
            }
        }

        public LogRow(int index, object?[] cells)
        {
            Index = index;
            Cells = cells;
        }

        public override string ToString()
        {
            return string.Join(" ", Cells);
        }
    }
}
