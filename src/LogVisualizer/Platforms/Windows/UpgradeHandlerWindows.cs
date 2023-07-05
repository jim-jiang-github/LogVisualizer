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
using System.Diagnostics;

namespace LogVisualizer.Platforms.Windows
{
    public class UpgradeHandlerWindows : UpgradeHandlerPlatform
    {
        public UpgradeHandlerWindows(INotify notify) : base(notify)
        {
        }

        public override string UpgradeResourceName { get; } = "win-x64.zip";

        public override string ExecutableFolder => AppDomain.CurrentDomain.BaseDirectory;

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
        public override void DoUpgrade(string upgradeScriptPath, string originalFolder, string targetFolder, string executablePath, bool needRestart)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = upgradeScriptPath,
                    Arguments = $"{originalFolder} {targetFolder} {executablePath} {needRestart}",
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            Log.Information($"process.StartInfo.Arguments is: {process.StartInfo.Arguments}");
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                FileOperationsHelper.SafeDeleteDirectory(originalFolder);
            }
        }
    }
}

