using LogVisualizer.Commons;
using LogVisualizer.Scenarios;
using LogVisualizer.Scenarios.Contents;
using LogVisualizer.Scenarios.Schemas;
using Serilog.Context;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class ScenarioService
    {
        private readonly IEnumerable<Scenario> _scenarios = Array.Empty<Scenario>();

        public string[] SupportedLogExtension { get; } = new[] { "*.log", "*.txt", "*.7z", "*.zip" };

        public ScenarioService()
        {
            if (!Directory.Exists(Global.SchemaConfigFolderRoot))
            {
                return;
            }
            _scenarios = Directory.GetDirectories(Global.SchemaConfigFolderRoot)
                .Select(directory => Scenario.LoadFromFolder(directory))
                .OfType<Scenario>()
                .ToArray();
        }
        public async Task OpenLogSource(string[] logSourcePath)
        {
            //foreach (var scenario in _scenarios)
            //{
            //    await scenario.OpenLog(logSourcePath);
            //}
        }
    }
}
