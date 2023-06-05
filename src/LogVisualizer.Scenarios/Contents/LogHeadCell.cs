using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Contents
{
    internal struct LogHeadCell
    {
        public string Name { get; set; }
        public object Cell { get; set; }
        internal LogHeadCell(string name, object cell)
        {
            Name = name;
            Cell = cell;
        }
    }
}
