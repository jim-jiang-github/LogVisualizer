using LogVisualizer.Scenarios.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LogVisualizer.Scenarios.Scenario;

namespace LogVisualizer.Scenarios
{
    public class Scenario
    {
        internal class LoadStep
        {
            public string SupportedExtension { get; }
            public string[] FileNameValidateRegexs { get; }
            public LogReader Reader { get; }

            public LoadStep(string supportedExtension, string[] fileNameValidateRegexs, LogReader reader)
            {
                SupportedExtension = supportedExtension;
                FileNameValidateRegexs = fileNameValidateRegexs;
                Reader = reader;
            }

            public bool IsFileValid(string filePath)
            {
                var extension = Path.GetExtension(filePath).TrimStart('.');
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                if (extension != SupportedExtension)
                {
                    return false;
                }
                if (!FileNameValidateRegexs.Any(x => Regex.IsMatch(fileName, x)))
                {
                    return false;
                }
                return true;
            }
        }
        public static Scenario? LoadFromFolder(string folder)
        {
            var schemaLogPath = Path.Combine(folder, "schema_log.json");
            var loader = SchemalogReader.GetSchemalogReaderFromJsonFile(schemaLogPath);
            if (loader == null)
            {
                return null;
            }
            //var loadSteps = loader.LoadSteps.Select(x => new LoadStep(x.SupportedExtension, x.FileNameValidateRegexs, x.ExtensionLoaderType, LogReader.GetReader(x.ReaderType)));
            //var supportedExtension = loader.LoadSteps.Select(x => x.SupportedExtension).ToArray();
            //var extensionMap = loader.LoadSteps.ToDictionary(x => x.SupportedExtension, x => x.ExtensionLoaderType);
            //var scenario = new Scenario(supportedExtension, loadSteps, extensionMap);
            return null;
        }

        private IEnumerable<LoadStep> _loadSteps;

        public string[] SupportedExtensions { get; private set; }


        private Scenario(string[] supportedExtensions, IEnumerable<LoadStep> loadSteps)
        {
            SupportedExtensions = supportedExtensions;
            _loadSteps = loadSteps;
        }

        public Task OpenLog(string logPath)
        {
            var step = _loadSteps.Where(x => x.IsFileValid(logPath)).FirstOrDefault();
            if (step == null)
            {
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
        public class Selector
        {

        }
        public class ASDASD
        {
            public Task LoadLogSource(string sourcePath, Action<Selector> selector)
            {
                return Task.CompletedTask;
            }
        }
    }
}
