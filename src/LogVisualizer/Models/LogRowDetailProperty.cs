using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Models
{
    public class LogRowDetailProperty : ModelBase
    {
        public string? PropertyName { get; set; }
        public string? PropertyValue { get; set; }
    }
}
