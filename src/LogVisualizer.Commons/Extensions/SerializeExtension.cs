using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons.Extensions
{
    public static class SerializeExtension
    {
        /// <summary>
        /// Implement save method
        /// </summary>
        public interface IJsonSerializable
        {
            public static T? GetAnonymousTypeFromJsonContent<T>(dynamic anonymousType, Func<string, T> deserializeAnonymousTypeCallback, string jsonContent)
            {
                try
                {
                    return deserializeAnonymousTypeCallback.Invoke(jsonContent);
                }
                catch (Exception ex)
                {
                    Log.Warning("Load error {error message}.", ex);
                    return default(T);
                }
            }
            public static string? LoadContentFromJsonFile(string jsonFilePath)
            {
                if (!File.Exists(jsonFilePath))
                {
                    Log.Warning("Not found json file in {jsonFilePath}.", jsonFilePath);
                    return null;
                }
                var jsonContent = File.ReadAllText(jsonFilePath);
                return jsonContent;
            }
            public static T? LoadFromJsonFile<T>(string jsonFilePath)
                where T : class
            {
                var jsonContent = LoadContentFromJsonFile(jsonFilePath);
                if (jsonContent == null)
                {
                    return null;
                }
                try
                {
                    var binaryContentParser = JsonConvert.DeserializeObject<T>(jsonContent);
                    return binaryContentParser;
                }
                catch (Exception ex)
                {
                    Log.Information("Load error {error message}.", ex);
                    return null;
                }
            }
        }
        public static string SaveAsJson(this IJsonSerializable jsonSerializable, string jsonFilePath)
        {
            var jsonContent = JsonConvert.SerializeObject(jsonSerializable, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(jsonFilePath, jsonContent);
            return jsonFilePath;
        }
    }
}
