using Avalonia.Controls;
using LogVisualizer.Commons;
using LogVisualizer.Scenarios.Scenarios;
using LogVisualizer.Scenarios.Schemas.Scenarios;
using LogVisualizer.Services;

namespace LogVisualizer.Test
{
    public class ScenarioTest
    {
        [Theory]
        [InlineData(@"Samples\1\1.log", @"Samples\1\Scenarios", 12)]
        //[InlineData("xxxx", false)]
        public async Task Pull(string logFilePath, string scenariosFolder, int rowCount)
        {
            Scenario scenario = new Scenario();
            scenario.Init(scenariosFolder);
            var result = scenario.LoadLogSource(logFilePath);
            Assert.True(result);
            Assert.Equal(scenario.LogSource?.TotalRowsCount, rowCount);
        }
    }
}