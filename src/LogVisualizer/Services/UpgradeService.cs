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
using LogVisualizer.I18N;
using MessageBox.Avalonia.DTO;
using Avalonia.Controls;
using MessageBox.Avalonia.Models;
using LogVisualizer.Commons.Notifications;
using static LogVisualizer.Commons.Notifications.Notify;
using static GithubReleaseUpgrader.UpgradeProgress;

namespace LogVisualizer.Services
{
    public class UpgradeService
    {
        //TODO only support on windows
        public class UpgradeProgressImpl : UpgradeProgress
        {
            private readonly UpgradeService _upgradeService;

            public override string UpgradeTempFolder { get; } = Global.UpgradeResourcesFolder;

            public override string GithubUrl { get; } = Global.GITHUB_URL;

            public override string UpgradeResourceName { get; } = "windows-x64.zip";

            public override string UpgradeInfoName { get; } = "upgradeInfo.json";

            public override string ExecutableName { get; } = $"{Global.APP_NAME}.exe";

            public override async Task Force(Version currentVersion, Version newtVersion, string? releaseLogMarkDown, ForceUpgradeHandle forceUpgradeHandle)
            {
                var title = I18NKeys.Upgrader_Upgrade_Version_Title.GetLocalizationString($"v{newtVersion}");
                var content = releaseLogMarkDown;
                var buttonNext = I18NKeys.Upgrader_Upgrade_Next_Startup.GetLocalizationRawValue();
                var buttonUpgrade = I18NKeys.Upgrader_Upgrade_Now.GetLocalizationRawValue();
                var result = await LogVisualizer.Commons.Notifications.Notify.ShowMessageBox(title, content, new MessageBoxButton(buttonNext), new MessageBoxButton(buttonUpgrade, true));

                if (result == buttonUpgrade)
                {
                    forceUpgradeHandle.UpgradeNow = true;
                    forceUpgradeHandle.NeedRestart = true;
                    return;
                }
                forceUpgradeHandle.UpgradeNow = false;
                forceUpgradeHandle.NeedRestart = false;
            }

            public override async Task Notify(Version currentVersion, Version newtVersion, string? releaseLogMarkDown, NotifyUpgradeHandle notifyUpgradeHandle)
            {
                var title = I18NKeys.Upgrader_Upgrade_Version_Title.GetLocalizationString($"v{newtVersion}");
                var content = releaseLogMarkDown;
                var buttonIgnore = I18NKeys.Upgrader_Ignore_This_Version.GetLocalizationRawValue();
                var buttonNext = I18NKeys.Upgrader_Upgrade_Next_Startup.GetLocalizationRawValue();
                var buttonDownload = I18NKeys.Upgrader_Download_Now.GetLocalizationRawValue();

                var result = await LogVisualizer.Commons.Notifications.Notify.ShowMessageBox(title, content, new MessageBoxButton(buttonIgnore), new MessageBoxButton(buttonNext), new MessageBoxButton(buttonDownload, true));

                if (result == buttonIgnore)
                {
                    notifyUpgradeHandle.NeedRestart = false;
                    notifyUpgradeHandle.Ignore = true;
                    notifyUpgradeHandle.Cancel = false;
                    return;
                }
                if (result == buttonNext)
                {
                    notifyUpgradeHandle.NeedRestart = false;
                    notifyUpgradeHandle.Ignore = false;
                    notifyUpgradeHandle.Cancel = false;
                    return;
                }
                if (result == buttonDownload)
                {
                    notifyUpgradeHandle.NeedRestart = true;
                    notifyUpgradeHandle.Ignore = false;
                    notifyUpgradeHandle.Cancel = false;
                    return;
                }
                notifyUpgradeHandle.NeedRestart = false;
                notifyUpgradeHandle.Ignore = false;
                notifyUpgradeHandle.Cancel = true;
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
