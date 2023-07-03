using System;
using Avalonia.Threading;
using GithubReleaseUpgrader;
using LogVisualizer.I18N;
using LogVisualizer.Services;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace LogVisualizer.Commons
{
    public abstract class UpgradeHandlerPlatform : UpgradeHandler
    {
        private readonly INotify _notify;

        public override string UpgradeTempFolder { get; } = Global.UpgradeResourcesFolder;

        public override string GithubUrl { get; } = Global.GITHUB_URL;

        public override string UpgradeInfoName { get; } = "upgradeInfo.json";

        public UpgradeHandlerPlatform(INotify notify)
        {
            _notify = notify;
        }

        public override async Task Force(Version currentVersion, Version newtVersion, string? releaseLogMarkDown, ForceUpgradeHandle forceUpgradeHandle)
        {
            var title = I18NKeys.Upgrader_Title.GetLocalizationString($"v{newtVersion}");
            var content = releaseLogMarkDown;
            var buttonNext = I18NKeys.Upgrader_UpgradeNextStartup.GetLocalizationRawValue();
            var buttonUpgrade = I18NKeys.Upgrader_UpgradeNow.GetLocalizationRawValue();
            var result = await _notify.ShowMessageBox(title, content, new MessageBoxButton(buttonNext), new MessageBoxButton(buttonUpgrade, true));

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
            var title = I18NKeys.Upgrader_Title.GetLocalizationString($"v{newtVersion}");
            var content = releaseLogMarkDown;
            var buttonIgnore = I18NKeys.Upgrader_IgnoreThisVersion.GetLocalizationRawValue();
            var buttonNext = I18NKeys.Upgrader_UpgradeNextStartup.GetLocalizationRawValue();
            var buttonDownload = I18NKeys.Upgrader_DownloadNow.GetLocalizationRawValue();

            var result = await _notify.ShowMessageBox(title, content, new MessageBoxButton(buttonIgnore), new MessageBoxButton(buttonNext), new MessageBoxButton(buttonDownload, true));

            if (result == buttonIgnore)
            {
                notifyUpgradeHandle.Donwload = false;
                notifyUpgradeHandle.Ignore = true;
                notifyUpgradeHandle.Cancel = false;
                return;
            }
            if (result == buttonNext)
            {
                notifyUpgradeHandle.Donwload = false;
                notifyUpgradeHandle.Ignore = false;
                notifyUpgradeHandle.Cancel = false;
                return;
            }
            if (result == buttonDownload)
            {
                notifyUpgradeHandle.Donwload = true;
                notifyUpgradeHandle.Ignore = false;
                notifyUpgradeHandle.Cancel = false;
                return;
            }
            notifyUpgradeHandle.Donwload = false;
            notifyUpgradeHandle.Ignore = false;
            notifyUpgradeHandle.Cancel = true;
        }

        public override void Tip(Version currentVersion, Version newtVersion, string? releaseLogMarkDown)
        {
        }

        public override void Shutdown()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                lifetime?.Shutdown();
            });
        }
    }
}

