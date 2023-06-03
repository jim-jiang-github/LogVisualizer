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
    public class ScenarioTest
    {
        [Theory]
        [InlineData(@"Samples\1\1.log", @"Samples\1\Scenarios", 12)]
        public void Scenario(string logFilePath, string scenariosFolder, int rowCount)
        {
            Scenario scenario = new Scenario();
            scenario.Init(scenariosFolder);
            var result = scenario.LoadLogSource(logFilePath);
            Assert.True(result);
            Assert.Equal(scenario.LogSource.RowsCount, rowCount);
        }
        [Theory]
        [InlineData(@"Samples\2\1.log", @"Samples\2\Scenarios", 999999)]
        public void Pull(string logFilePath, string scenariosFolder, int rowCount)
        {
            var line = File.ReadAllText(logFilePath);
            using var autoDelete = FileOperationsHelper.CreateAutoDelete(".log");
            var randomFile = autoDelete.RandomFile;
            using var fileStream = File.OpenWrite(randomFile);
            using var streamWriter = new StreamWriter(fileStream);
            for (int i = 0; i < rowCount; i++)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Dispose();
            Scenario scenario = new Scenario();
            scenario.Init(scenariosFolder);
            var result = scenario.LoadLogSource(randomFile);
            Assert.True(result);
            Assert.Equal(scenario.LogSource?.RowsCount, rowCount);
        }
    }
}