using Commons;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogVisualizer
{
    public class Configuration
    {
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
            var configPath = Path.Combine(Global.CurrentAppDataDirectory, CONFIG_FILE_NAME);
            if (!File.Exists(configPath))
            {
                _instance = new Configuration();
                return;
            }
            try
            {
                var json = File.ReadAllText(configPath);
                var configuration = JsonSerializer.Deserialize<Configuration>(json);
                _instance = configuration ?? new Configuration();
                return;
            }
            catch (Exception ex)
            {
                Log.Error("Configuration load from json error hanppened:{ex} raw json:{json}", ex, configPath);
                _instance = new Configuration();
                return;
            }
        }

        public string? SchemaConfigRepo { get; set; }

        public void Save()
        {
            var configPath = Path.Combine(Global.CurrentAppDataDirectory, CONFIG_FILE_NAME);
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(configPath, json);
        }
    }
}
