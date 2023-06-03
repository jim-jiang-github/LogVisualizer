using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;
using LogVisualizer.Scenarios.Sources;

namespace LogVisualizer.Scenarios
{
    public class Scenario : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private LogLoader? _streamLoader;
        private string? _schemaLogPath;

        public string[] SupportedExtensions { get; private set; } = Array.Empty<string>();
        public string[] LoadedLogFiles { get; private set; } = Array.Empty<string>();
        public ILogSource? LogSource { get; private set; }
        public bool Initialized => SupportedExtensions.Length > 0;
        public bool LogSourceLoaded => LogSource != null;
        public Scenario()
        {

        }
        protected void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool Init(string scenariosFolder)
        {
            if (!Directory.Exists(scenariosFolder))
            {
                return false;
            }
            var files = Directory.GetFiles(scenariosFolder);
            var schemaTypeMap = files.ToDictionary(f => Schema.GetSchemaTypeFromJsonFile(f), f => f);
            var schemaScenarioCount = schemaTypeMap.Count(x => x.Key == SchemaType.Scenario);
            if (schemaScenarioCount == 0)
            {
                Log.Warning("Can not found schemaScenario in {scenarioDirectory}", scenariosFolder);
                return false;
            }
            if (schemaScenarioCount > 1)
            {
                Log.Warning("Found multiple schemaScenario in {scenarioDirectory}", scenariosFolder);
                return false;
            }
            var schemaScenarioPath = schemaTypeMap.FirstOrDefault(x => x.Key == SchemaType.Scenario).Value;
            var schemaScenario = IJsonSerializable.LoadFromJsonFile<SchemaScenario>(schemaScenarioPath);
            if (schemaScenario == null)
            {
                Log.Warning("Can not load schemaScenario from json file in {schemaScenarioPath}", schemaScenarioPath);
                return false;
            }
            if (schemaScenario.SchemaLogName == string.Empty)
            {
                Log.Warning("Schema scenario do not have schema log name");
                return false;
            }
            var schemaLogPath = Path.Combine(scenariosFolder, schemaScenario.SchemaLogName);
            var schemaLogContent = IJsonSerializable.LoadContentFromJsonFile(schemaLogPath);
            if (schemaLogContent == null)
            {
                Log.Warning("Can not load schema log content from json file in {schemaLogPath}", schemaLogPath);
                return false;
            }
            var supportExtensions = GetSupportedExtensionsFromJsonContent(schemaLogContent);
            if (supportExtensions.Length <= 0)
            {
                Log.Warning("Can not support any extensions from json file in {schemaLogPath}", schemaLogPath);
                return false;
            }
            var logFileLoaderType = GetLogFileLoaderTypeFromJsonContent(schemaLogContent);
            var streamLoader = GetLoader(logFileLoaderType);
            if (streamLoader == null)
            {
                Log.Warning("Can not load stream from json file by {loader} in {schemaLogContent}", streamLoader, schemaLogContent);
                return false;
            }
            _schemaLogPath = schemaLogPath;
            _streamLoader = streamLoader;
            SupportedExtensions = supportExtensions;
            Log.Information("Scenario inited");
            //LoadLogSource(@"C:\Users\Jim.Jiang\Downloads\WRoomsFeedBack_HostLog_75a6889c-ce51-4840-ace1-3ef098034520_20220615-104915\RoomsHost-20220614_132503-pid_3220.log");
            return true;
        }

        public void Dispose()
        {
            LogSource?.Dispose();
            LogSource = null;
            _schemaLogPath = null;
            _streamLoader = null;
            SupportedExtensions = Array.Empty<string>();
        }
        public void LoadLogFiles(string[] logFiles)
        {
            if (logFiles.Length == 0)
            {
                return;
            }
            LoadedLogFiles = logFiles;
            LoadLogSource(logFiles[0]);
            OnPropertyChanged(nameof(LoadedLogFiles));
        }
        public bool LoadLogSource(string logFilePath)
        {
            if (!Initialized)
            {
                Log.Warning("No initialized");
                return false;
            }
            if (_streamLoader == null || _schemaLogPath == null)
            {
                return false;
            }
            if (LogSourceLoaded)
            {
                Log.Information("Reset LogSource");
                LogSource?.Dispose();
                LogSource = null;
                GC.Collect();
                OnPropertyChanged(nameof(LogSource));
            }
            Log.Information("Load LogSource from {logFilePath}", logFilePath);
            var stream = _streamLoader.Load(logFilePath);
            LogSource = ILogSource.LoadLogSource(stream, _schemaLogPath);
            OnPropertyChanged(nameof(LogSource));
            return true;
        }
    }
}
