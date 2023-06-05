using LogVisualizer.Scenarios.Schemas;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogVisualizer.Commons.Extensions.SerializeExtension;

namespace LogVisualizer.Services
{
    public class ScenarioService
    {
        public async Task<bool> LoadFromFolder(string scenariosFolder)
        {
            await Task.Delay(1);
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
            //var supportExtensions = GetSupportedExtensionsFromJsonContent(schemaLogContent);
            //if (supportExtensions.Length <= 0)
            //{
            //    Log.Warning("Can not support any extensions from json file in {schemaLogPath}", schemaLogPath);
            //    return false;
            //}
            //var logFileLoaderType = GetLogFileLoaderTypeFromJsonContent(schemaLogContent);
            //var streamLoader = GetLoader(logFileLoaderType);
            //if (streamLoader == null)
            //{
            //    Log.Warning("Can not load stream from json file by {loader} in {schemaLogContent}", streamLoader, schemaLogContent);
            //    return false;
            //}
            //_schemaLogPath = schemaLogPath;
            //_streamLoader = streamLoader;
            //SupportedExtensions = supportExtensions;
            //Log.Information("Scenario inited");
            //LoadLogSource(@"C:\Users\Jim.Jiang\Downloads\WRoomsFeedBack_HostLog_75a6889c-ce51-4840-ace1-3ef098034520_20220615-104915\RoomsHost-20220614_132503-pid_3220.log");
            return true;
        }
    }
}
