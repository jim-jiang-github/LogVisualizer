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
    public partial class ScenarioConfig : ModelBase
    {
        [ObservableProperty]
        private string? _scenarioName = string.Empty;
        [ObservableProperty]
        private string? _scenarioRepo = string.Empty;
        [ObservableProperty]
        private string? _scenarioBranch = string.Empty;
        [ObservableProperty]
        private bool _hasUpdate = false;
        [ObservableProperty]
        private ObservableCollection<string> _filterBranches = new ObservableCollection<string>();
        public string ScenarioConfigFolder => System.IO.Path.Combine(Global.ScenarioConfigFolderRoot, ScenarioName);
    }
}
