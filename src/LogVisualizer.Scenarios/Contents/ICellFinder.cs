using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Contents
{
    internal interface ICellFinder
    {
        object? GetCellValue(string recursivePath);
    }
}
