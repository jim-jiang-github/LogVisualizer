using Commons;
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
        public event EventHandler<Version, Version, string?> UpgradeNotify;

        //TODO only support on windows
        public class UpgradeProgressImpl : UpgradeProgress
        {
            private readonly UpgradeService _upgradeService;

            public override string UpgradeTempFolder { get; } = Path.Combine(Global.CurrentAppDataDirectory, "UpgraderTemp");

            public override string GithubUrl { get; } = Global.GITHUB_URL;

            public override string UpgradeResourceName { get; } = "windows-x64.zip";

            public override string UpgradeInfoName { get; } = "upgradeInfo.json";

            public override string ExecutableName { get; } = $"{Global.APP_NAME}.exe";

            public override void Force(Version currentVersion, Version newtVersion, string? releaseLogMarkDown)
            {
                _upgradeService.UpgradeNotify?.Invoke(this, currentVersion, newtVersion, releaseLogMarkDown);
            }

            public override UpgradeOption Notify(Version currentVersion, Version newtVersion, string? releaseLogMarkDown)
            {

                return UpgradeOption.Cancel;
            }

            public override void Tip(Version currentVersion, Version newtVersion, string? releaseLogMarkDown)
            {
            }

            public UpgradeProgressImpl(UpgradeService upgradeService)
            {
                _upgradeService = upgradeService;
            }
        }

        public bool CheckForUpgrade()
        {
            var isNeedUpgrade = Upgrader.CheckForUpgrade(new UpgradeProgressImpl(this));
            return isNeedUpgrade;
        }

        public void PerformUpgradeIfNeeded()
        {
            Upgrader.PerformUpgradeIfNeeded();
        }
    }
}
