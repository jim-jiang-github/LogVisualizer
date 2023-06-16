using System;
using Avalonia.Threading;
using GithubReleaseUpgrader;
using LogVisualizer.Commons;
using LogVisualizer.I18N;
using LogVisualizer.Services;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace LogVisualizer.Platforms.Windows
{
    public class UpgradeProgressWindows : UpgradeProgressPlatform
    {
        public override string UpgradeResourceName { get; } = "windows-x64.zip";

        public override string ExecutableName { get; } = $"{Global.APP_NAME}.exe";
    }
}

