using LogVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Models
{
    public class LogItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsLoading { get; set; }
    }
}
