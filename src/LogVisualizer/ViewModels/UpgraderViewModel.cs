using Commons.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class UpgraderViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _upgradeChangedLog;
        [ObservableProperty]
        private bool _isVisible = false;

#if DEBUG
        public UpgraderViewModel()
        {

        }
#endif

        public UpgraderViewModel(UpgradeService upgradeService)
        {
            upgradeService.UpgradeNotify += (sender, currentVersion, newtVersion, releaseLogMarkDown) =>
            {
                UpgradeChangedLog = releaseLogMarkDown;
                IsVisible = true;
            };
        }
    }
}
