using LogVisualizer.Decompress;
using LogVisualizer.I18N;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class I18NTest
    {
        public I18NTest()
        {
        }

        [Theory]
        [InlineData(@"..\LogVisualizer.I18N\I18NResources\en-US.json", 12)]
        [InlineData(@"..\LogVisualizer.I18N\I18NResources\non-localized.json", 12)]
        public async Task I18NKeysTest(string i18nJsonPath, int expected)
        {
            var i18nJsonContent = File.ReadAllText(i18nJsonPath);
            var localizationKey = GetLocalizationKey(i18nJsonContent);
            //Assert.Equal(expected, count);
        }

        //[Theory]
        //[InlineData(@"..\LogVisualizer.I18N\I18NResources\en-US.json", 12)]
        //[InlineData(@"..\LogVisualizer.I18N\I18NResources\non-localized.json", 12)]
        //public async Task ReadCompressedPackage(string compressedPackagePath)
        //{
        //    var paths = CompressedPackageLoader.GetEntryPaths(compressedPackagePath);
        //    foreach (string path in paths)
        //    {
        //        using var stream = CompressedPackageReader.ReadStream(path);
        //        StreamReader reader = new StreamReader(stream);
        //        var newPath = path;
        //        int delimiterIndex = path.LastIndexOf("|");
        //        if (delimiterIndex != -1)
        //        {
        //            newPath = newPath.Substring(delimiterIndex + 1);
        //        }
        //        var fileName = Path.GetFileNameWithoutExtension(newPath);
        //        var content = reader.ReadLine();
        //        Assert.Equal(fileName, content);
        //    }
        //}

        private IEnumerable<string> GetLocalizationKey(string? jsonContent, string? parentKey = null)
        {
            if (jsonContent != null)
            {
                var jsonObjects = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                if (jsonObjects != null)
                {
                    foreach (var jsonObject in jsonObjects)
                    {
                        var key = (parentKey == null ? "" : $"{parentKey}_") + jsonObject.Key;
                        if (jsonObject.Key == "Plurals" && parentKey != null)
                        {
                            yield return parentKey;
                            continue;
                        }
                        if (jsonObject.Value is string value)
                        {
                            yield return key;
                            continue;
                        }
                        foreach (var item in GetLocalizationKey(jsonObject.Value.ToString(), key))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetLocalizationMap(string? jsonContent, string? parentKey = null)
        {
            if (jsonContent != null)
            {
                var jsonObjects = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                if (jsonObjects != null)
                {
                    foreach (var jsonObject in jsonObjects)
                    {
                        var key = (parentKey == null ? "" : $"{parentKey}_") + jsonObject.Key;
                        if (jsonObject.Key == "Plurals" && jsonObject.Value is string valuePlurals && Plurals.LoadFromJson(valuePlurals) is Plurals plurals)
                        {
                            yield return new KeyValuePair<string, object>(key, plurals);
                            continue;
                        }
                        if (jsonObject.Value is string value)
                        {
                            yield return new KeyValuePair<string, object>(key, value);
                            continue;
                        }
                        foreach (var item in GetLocalizationMap(jsonObject.Value.ToString(), key))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }
    }
}
