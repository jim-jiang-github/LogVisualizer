using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public static class Global
    {
        public const string APP_NAME = "LogVisualizer";

        public const string GITHUB_URL = "https://github.com/jim-jiang-github/LogVisualizer";

        public static string CurrentAppDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APP_NAME);
    }
}