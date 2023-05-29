using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public static class Global
    {
        public const string APP_NAME = "LogVisualizer";

        public const string GITHUB_URL = "https://github.com/jim-jiang-github/LogVisualizer";

        public static string AppTempDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), APP_NAME);

        public static string AppDataDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APP_NAME);

        public static string UpgradeResourcesFolder { get; } = Path.Combine(AppDataDirectory, "UpgraderResources");

        public static string SchemaConfigFolderRoot { get; } = Path.Combine(AppDataDirectory, "SchemaConfigs");
    }
}