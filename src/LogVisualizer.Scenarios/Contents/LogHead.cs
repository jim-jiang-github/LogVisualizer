using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Contents
{
    internal struct LogHead
    {
        private readonly LogHeadCell[] _logHeadCells;

        public string Name { get; }
        public int Count => _logHeadCells.Length;

        public object? this[int index]
        {
            get
            {
                return _logHeadCells[index].Cell;
            }
        }

        public string GetCellName(int index)
        {
            return _logHeadCells[index].Name;
        }

        internal LogHead(string name, LogHeadCell[] logHeadCells)
        {
            Name = name;
            _logHeadCells = logHeadCells;
        }
    }
}
