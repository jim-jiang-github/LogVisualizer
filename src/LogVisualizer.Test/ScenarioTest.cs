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
        public ScenarioTest()
        {
        }

        [Theory]
        [InlineData(@"Samples\Scenarios\x", false)]
        [InlineData(@"Samples\Scenarios\1", true)]
        [InlineData(@"Samples\Scenarios\x\", false)]
        [InlineData(@"Samples\Scenarios\1\", true)]
        public void ScenarioLoadTest(string scenariosFolder, bool excepted)
        {
            Scenario? scenario = Scenario.LoadFromFolder(scenariosFolder);
            var result = scenario != null;
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(@"Samples\Scenarios\1\", new[] { "*.log", "*.txt" })]
        public void ScenarioSupportedExtensionsTest(string scenariosFolder, string[] excepted)
        {
            Scenario? scenario = Scenario.LoadFromFolder(scenariosFolder);
            if (scenario == null)
            {
                Assert.Fail("scenario is null");
            }
            var result = scenario.SupportedExtensions;
            Assert.Equal(excepted, result);
        }

        //[Theory]
        //[InlineData(@"Samples\Scenarios\1\1.log", @"Samples\Scenarios\1", 12)]
        //public void ScenarioLoadTest(string logFilePath, string scenariosFolder, int rowCount)
        //{
        //    Scenario? scenario = Scenario.LoadFromFolder(scenariosFolder);
        //    scenario.Init(scenariosFolder);
        //    var result = scenario.LoadLogContent(logFilePath);
        //    Assert.True(result);
        //    Assert.Equal(scenario.LogContent.RowsCount, rowCount);
        //}

        //[Theory]
        //[InlineData(@"Samples\2\1.log", @"Samples\2\Scenarios", 999999)]
        //public void Pull(string logFilePath, string scenariosFolder, int rowCount)
        //{
        //    var line = File.ReadAllText(logFilePath);
        //    using var autoDelete = FileOperationsHelper.CreateAutoDelete(".log");
        //    var randomFile = autoDelete.RandomFile;
        //    using var fileStream = File.OpenWrite(randomFile);
        //    using var streamWriter = new StreamWriter(fileStream);
        //    for (int i = 0; i < rowCount; i++)
        //    {
        //        streamWriter.WriteLine(line);
        //    }
        //    streamWriter.Dispose();
        //    Scenario scenario = new Scenario();
        //    scenario.Init(scenariosFolder);
        //    Stopwatch stopwatch = Stopwatch.StartNew();
        //    var result = scenario.LoadLogContent(randomFile);
        //    var durtion = stopwatch.ElapsedMilliseconds;
        //    stopwatch.Stop();
        //    Assert.True(result);
        //    Assert.Equal(scenario.LogContent?.RowsCount, rowCount);
        //}
    }
}