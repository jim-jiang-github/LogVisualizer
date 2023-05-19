using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Commons.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.I18N;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GithubReleaseUpgrader.UpgradeProgress;

namespace LogVisualizer.ViewModels
{
    public partial class UpgraderViewModel : ViewModelBase
    {
        private enum UpgradeMode
        {
            Ignore,
            UpgradeNextStartup,
            UpgradeNow
        }

        [ObservableProperty]
        private string? _upgradeChangedLog;
        [ObservableProperty]
        private string? _version;
        [ObservableProperty]
        private bool _isVisible = false;
        [ObservableProperty]
        private bool _canIgnore = false;

        private TaskCompletionSource<UpgradeMode> _taskUpgradeModeCompletionSource = new TaskCompletionSource<UpgradeMode>();

#if DEBUG
        public UpgraderViewModel()
        {

        }
#endif

        public UpgraderViewModel(UpgradeService upgradeService)
        {
            upgradeService.UpgradeForce += async (sender, newtVersion, releaseLogMarkDown, forceUpgradeHandle) =>
            {
                Version = $"v{newtVersion}";
                UpgradeChangedLog = releaseLogMarkDown;
                CanIgnore = false;
                IsVisible = true;
                var upgradeMode = await _taskUpgradeModeCompletionSource.Task;
                if (upgradeMode == UpgradeMode.UpgradeNow)
                {
                    forceUpgradeHandle.UpgradeNow = true;
                    forceUpgradeHandle.NeedRestart = true;
                }
                else
                {
                    forceUpgradeHandle.UpgradeNow = false;
                    forceUpgradeHandle.NeedRestart = false;
                }
                IsVisible = false;
                _taskUpgradeModeCompletionSource = new TaskCompletionSource<UpgradeMode>();
            };
            upgradeService.UpgradeNotify += async (sender, newtVersion, releaseLogMarkDown, notifyUpgradeHandle) =>
            {
                Version = $"v{newtVersion}";
                UpgradeChangedLog = releaseLogMarkDown;
                CanIgnore = true;
                IsVisible = true;
                var upgradeMode = await _taskUpgradeModeCompletionSource.Task;
                switch (upgradeMode)
                {
                    case UpgradeMode.UpgradeNow:
                        notifyUpgradeHandle.UpgradeNow = true;
                        notifyUpgradeHandle.NeedRestart = true;
                        notifyUpgradeHandle.Ignore = false;
                        break;
                    case UpgradeMode.UpgradeNextStartup:
                        notifyUpgradeHandle.UpgradeNow = false;
                        notifyUpgradeHandle.NeedRestart = false;
                        notifyUpgradeHandle.Ignore = false;
                        break;
                    case UpgradeMode.Ignore:
                    default:
                        notifyUpgradeHandle.UpgradeNow = false;
                        notifyUpgradeHandle.NeedRestart = false;
                        notifyUpgradeHandle.Ignore = true;
                        break;
                }
                IsVisible = false;
                _taskUpgradeModeCompletionSource = new TaskCompletionSource<UpgradeMode>();
            };
        }

        [RelayCommand]
        public void IgnoreThisVersion()
        {
            _taskUpgradeModeCompletionSource.SetResult(UpgradeMode.Ignore);
        }

        [RelayCommand]
        public void UpgradeNextStartup()
        {
            _taskUpgradeModeCompletionSource.SetResult(UpgradeMode.UpgradeNextStartup);
        }

        [RelayCommand]
        public void UpgradeNow()
        {
            _taskUpgradeModeCompletionSource.SetResult(UpgradeMode.UpgradeNow);
        }
    }
}
