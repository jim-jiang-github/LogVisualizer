using LogVisualizer.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static LogVisualizer.Commons.Extensions.SerializeExtension;

namespace LogVisualizer
{
    public class Configuration : IJsonSerializable
    {
        public class ScenarioConfiguration
        {
            public string? ScenarioName { get; set; }
            public string? ScenarioRepo { get; set; }
        }

        private const string CONFIG_FILE_NAME = "configuration.json";

        private static Configuration _instance;

        public static Configuration Instance => _instance;

        static Configuration()
        {
            CreateInstance();
        }

        public static void CreateInstance()
        {
            if (_instance != null)
            {
                return;
            }
            var configPath = Path.Combine(Global.AppDataDirectory, CONFIG_FILE_NAME);
            var configuration = IJsonSerializable.LoadFromJsonFile<Configuration>(configPath);
            _instance = configuration ?? new Configuration();
        }

        public ScenarioConfiguration? Scenario { get; set; }

        public void Save()
        {
            var configPath = Path.Combine(Global.AppDataDirectory, CONFIG_FILE_NAME);
            this.SaveAsJson(configPath);
        }
    }
}
