using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Sources
{
    internal interface IBlockCellFinder
    {
        object? GetBlockCellValue(string recursivePath);
    }
}
