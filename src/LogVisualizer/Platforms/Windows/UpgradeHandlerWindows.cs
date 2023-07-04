using System;
using Avalonia.Threading;
using GithubReleaseUpgrader;
using LogVisualizer.Commons;
using LogVisualizer.I18N;
using LogVisualizer.Services;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.IO;
using System.Reflection;

namespace LogVisualizer.Platforms.Windows
{
    public class UpgradeHandlerWindows : UpgradeHandlerPlatform
    {
        public UpgradeHandlerWindows(INotify notify) : base(notify)
        {
        }

        public override string UpgradeResourceName { get; } = "win-x64.zip";

        public override string ExecutableName { get; } = $"{Global.APP_NAME}.exe";

        public override string UpgradeScriptName => "upgrader.bat";

        protected override string? GetUpgradeScriptContent()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream($"LogVisualizer.Platforms.Windows.{UpgradeScriptName}");
            if (stream == null)
            {
                Log.Warning("Can not found upgrader");
                return null;
            }
            using StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            return content;
        }
    }
}

