using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class SchemaConfigViewModel : ViewModelBase
    {
        private GitService _gitService;

        [ObservableProperty]
        private string? _currentName;
        [ObservableProperty]
        private bool _needsAttention = false;
        [ObservableProperty]
        private ObservableCollection<string> _branches;

        public SchemaConfigViewModel(GitService gitService)
        {
            _gitService = gitService;
            CurrentName = "xdasd";
            _branches = new ObservableCollection<string>();
            NeedsAttention = true;

            Configuration.Instance.SchemaConfigRepo = "git@git.ringcentral.com:CoreLib/rcvrooms-windows.git";
            Task.Run(async () =>
            {
                var name = await gitService.GetCurrentBranchName(Configuration.Instance.SchemaConfigRepo);
                var branches = await gitService.GetAllOriginBranches(Configuration.Instance.SchemaConfigRepo);
                int a = 1;
            });
        }
    }
}
