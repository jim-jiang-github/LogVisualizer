using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using LogVisualizer.Commons;
using GithubReleaseUpgrader;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class UpgradeService
    {
        public event EventHandlerAsync<Version, string?, UpgradeProgress.ForceUpgradeHandle>? UpgradeForce;
        public event EventHandlerAsync<Version, string?, UpgradeProgress.NotifyUpgradeHandle>? UpgradeNotify;

        //TODO only support on windows
        public class UpgradeProgressImpl : UpgradeProgress
        {
            private readonly UpgradeService _upgradeService;

            public override string UpgradeTempFolder { get; } = Path.Combine(Global.CurrentAppDataDirectory, "UpgraderTemp");

            public override string GithubUrl { get; } = Global.GITHUB_URL;

            public override string UpgradeResourceName { get; } = "windows-x64.zip";

            public override string UpgradeInfoName { get; } = "upgradeInfo.json";

            public override string ExecutableName { get; } = $"{Global.APP_NAME}.exe";

            public override async Task Force(Version currentVersion, Version newtVersion, string? releaseLogMarkDown, ForceUpgradeHandle forceUpgradeHandle)
            {
                if (_upgradeService.UpgradeForce == null)
                {
                    return;
                }
                await _upgradeService.UpgradeForce.Invoke(this, newtVersion, releaseLogMarkDown, forceUpgradeHandle);
            }

            public override async Task Notify(Version currentVersion, Version newtVersion, string? releaseLogMarkDown, NotifyUpgradeHandle notifyUpgradeHandle)
            {
                if (_upgradeService.UpgradeNotify == null)
                {
                    return;
                }
                await _upgradeService.UpgradeNotify.Invoke(this, newtVersion, releaseLogMarkDown, notifyUpgradeHandle);
            }

            public override void Tip(Version currentVersion, Version newtVersion, string? releaseLogMarkDown)
            {
            }

            public override void Shutdown()
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                    lifetime?.Shutdown();
                });
            }

            public UpgradeProgressImpl(UpgradeService upgradeService)
            {
                _upgradeService = upgradeService;
            }
        }

        public bool CheckForUpgrade(bool forceCheck = false)
        {
            var isNeedUpgrade = Upgrader.CheckForUpgrade(new UpgradeProgressImpl(this), forceCheck);
            return isNeedUpgrade;
        }

        public void PerformUpgradeIfNeeded()
        {
            Upgrader.PerformUpgradeIfNeeded();
        }
    }
}
