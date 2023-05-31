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
        [InlineData("C:\\Users\\Jim.Jiang\\Downloads\\RoomsHost-20230529_102210777_pid-14704\\RoomsHost-20230529_102210777_pid-14704.log", "C:\\Users\\Jim.Jiang\\AppData\\Roaming\\Microsoft\\Windows\\Templates\\LogVisualizer\\TestFolder\\Schemas\\schema_log.json", true)]
        //[InlineData("xxxx", false)]
        public async Task Pull(string logFilePath, string schemaLogPath, bool expected)
        {
            //SchemaScenario schemaScenario = new SchemaScenario();
            //schemaScenario.SaveAsDefault();
            Scenario scenario = new Scenario();
            scenario.Init();
            var asd = scenario.LoadLogSource(logFilePath);

            //Assert.Equal(expected, result);
        }
    }
}