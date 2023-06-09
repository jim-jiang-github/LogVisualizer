using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using LogVisualizer.Commons;
using LogVisualizer.Scenarios;
using LogVisualizer.Services;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reactive.Concurrency;
using System.Text.RegularExpressions;

namespace LogVisualizer.Test
{
    public class ScenarioServiceTest
    {
        private ScenarioService _scenarioService;
        public ScenarioServiceTest(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        [Theory]
        [InlineData("")]
        public async Task LoadFromFolder(string logFilePath)
        {
            //await _scenarioService.lo(Global.SchemaConfigFolderRoot);
        }
    }
}