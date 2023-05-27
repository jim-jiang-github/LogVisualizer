using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Models
{
    public partial class SchemaConfig : ObservableObject
    {
        [ObservableProperty]
        private string? _schemaName = string.Empty;
        [ObservableProperty]
        private string? _schemaBranch = string.Empty;
    }
}
