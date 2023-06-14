using LogVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Models
{
    public class LogFileItem
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
        public ObservableCollection<LogFileItem>? Children { get; set; }
        public bool IsLoading { get; set; } = false;

        public LogFileItem(string? name, string? path, string? description, LogFileItem[]? subItems)
        {
            Name = name;
            Path = path;
            Description = description;
            if (subItems == null)
            {
                Children = null;
                return;
            }
            Children = new ObservableCollection<LogFileItem>(subItems);
        }
    }
}
