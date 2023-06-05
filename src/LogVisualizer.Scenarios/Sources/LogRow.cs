﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Sources
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

        internal LogRow(int index, object?[] cells)
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