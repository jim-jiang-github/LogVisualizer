﻿using LogVisualizer.I18N;
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
        [InlineData(@"../../../../../LogVisualizer.I18N/I18NResources/en.json")]
        [InlineData(@"../../../../../LogVisualizer.I18N/I18NResources/non-localized.json")]
        public void I18NKeysTest(string i18nJsonPath)
        {
            var i18nJsonContent = File.ReadAllText(i18nJsonPath);
            var localizationKey = GetLocalizationKey(i18nJsonContent);
            Assert.IsAssignableFrom<IEnumerable<string>>(localizationKey);
            var localizationMap = GetLocalizationMap(i18nJsonContent);
            Assert.IsAssignableFrom<IEnumerable<KeyValuePair<string, object>>>(localizationMap);
        }

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
