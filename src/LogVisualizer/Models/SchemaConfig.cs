using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string? _schemaRepo = string.Empty;
        [ObservableProperty]
        private string? _schemaBranch = string.Empty;
        [ObservableProperty]
        private bool _hasUpdate = false;
        [ObservableProperty]
        private ObservableCollection<string> _filterBranches = new ObservableCollection<string>();
        public string SchemaConfigFolder => System.IO.Path.Combine(Global.SchemaConfigFolderRoot, SchemaName);
    }
}
