using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public static class Global
    {
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public const string APP_NAME = "LogVisualizer";

        public const string GITHUB_URL = "https://github.com/jim-jiang-github/LogVisualizer";

        public static string CurrentVersionStr => CurrentVersion.ToString(3);

        public static Version CurrentVersion { get; } = typeof(Global).Assembly.GetName().Version ?? Version.Parse("1.0.0.0");

        public static string AppTempDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), APP_NAME);

        public static string AppDataDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APP_NAME);

        public static string UpgradeResourcesFolder { get; } = Path.Combine(AppDataDirectory, "UpgraderResources");

        public static string ScenarioConfigFolderRoot { get; } = Path.Combine(AppDataDirectory, "ScenarioConfigs");
    }
}