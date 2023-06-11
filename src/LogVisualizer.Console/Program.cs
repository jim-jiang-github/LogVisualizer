using LogVisualizer.Console;
using LogVisualizer.Decompress;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

IEnumerable<KeyValuePair<string, string>> GetLocalizationKey(string? jsonContent, string? parentKey = null)
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
                    var pluralsValue = (jsonObject.Value.ToString() ?? string.Empty).Replace("\n", "\\n").Replace("\r", "\\r");
                    yield return new KeyValuePair<string, string>(parentKey, pluralsValue);
                    continue;
                }
                if (jsonObject.Value is string value)
                {
                    yield return new KeyValuePair<string, string>(key, value);
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
IEnumerable<KeyValuePair<string, object>> GetLocalizationMap(string? jsonContent, string? parentKey = null)
{
    if (jsonContent != null)
    {
        var jsonObjects = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
        if (jsonObjects != null)
        {
            foreach (var jsonObject in jsonObjects)
            {
                var key = (parentKey == null ? "" : $"{parentKey}_") + jsonObject.Key;
                if (jsonObject.Key == "Plurals")
                {
                    yield return new KeyValuePair<string, object>(key, Plurals.LoadFromJson(jsonObject.Value.ToString()));
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

string GetLocalizationValue(string key, IEnumerable<KeyValuePair<string, object>> localizationMap)
{
    var value = localizationMap.FirstOrDefault(x => x.Key == key).Value;
    return "";
}

string jsonLocalized = File.ReadAllText(@"C:\Users\Jim.Jiang\Documents\LogVisualizer\src\LogVisualizer.I18N\I18NResources\en-AU.json");
var localizationMap = GetLocalizationKey(jsonLocalized).ToArray();
int i = 1;
//WriteEnum("xxxxxxxxxx", localizationMap);
//void WriteEnum(string head, IEnumerable<KeyValuePair<string, object>> localizationMap)
//{
//    foreach (var item in localizationMap)
//    {
//        var key = item.Key;
//        if (item.Value is string value)
//        {
//            value = value.Replace("\n", "\\n");
//            value = value.Replace("\r", "\\r");
//            WriteLine($"        /// <summary>{head}{value}</summary>");
//            WriteLine($"        {key},");
//            continue;
//        }
//        if (item.Value is Plurals plurals)
//        {
//            WriteLine($"        /// <summary>{head}This is a plurals value</summary>");
//            WriteLine($"        /// <remarks>");
//            if (plurals.Zero is string zero)
//            {
//                zero = zero.Replace("\n", "\\n");
//                zero = zero.Replace("\r", "\\r");
//                WriteLine($"        /// <para>");
//                WriteLine($"        /// [zero]: {zero}");
//                WriteLine($"        /// </para>");
//            }
//            if (plurals.One is string one)
//            {
//                one = one.Replace("\n", "\\n");
//                one = one.Replace("\r", "\\r");
//                WriteLine($"        /// <para>");
//                WriteLine($"        /// [one]: {one}");
//                WriteLine($"        /// </para>");
//            }
//            if (plurals.Two is string two)
//            {
//                two = two.Replace("\n", "\\n");
//                two = two.Replace("\r", "\\r");
//                WriteLine($"        /// <para>");
//                WriteLine($"        /// [two]: {two}");
//                WriteLine($"        /// </para>");
//            }
//            if (plurals.Few is string few)
//            {
//                few = few.Replace("\n", "\\n");
//                few = few.Replace("\r", "\\r");
//                WriteLine($"        /// <para>");
//                WriteLine($"        /// [few]: {few}");
//                WriteLine($"        /// </para>");
//            }
//            if (plurals.Many is string many)
//            {
//                many = many.Replace("\n", "\\n");
//                many = many.Replace("\r", "\\r");
//                WriteLine($"        /// <para>");
//                WriteLine($"        /// [many]: {many}");
//                WriteLine($"        /// </para>");
//            }
//            if (plurals.Other is string other)
//            {
//                other = other.Replace("\n", "\\n");
//                other = other.Replace("\r", "\\r");
//                WriteLine($"        /// <para>");
//                WriteLine($"        /// [other]: {other}");
//                WriteLine($"        /// </para>");
//            }
//            WriteLine($"        /// </remarks>");
//            WriteLine($"        {key},");
//            continue;
//        }
//    }
//}
//var value = GetLocalizationValue("errors_schemaValidationFailed_type", localizationMap);

var qwe1 = CompressedPackageLoader.IsSupportedCompressedPackage(@"C:\Users\Jim.Jiang\Downloads\RoomsHost-20230531_152036478_pid-21544.init.log.zip");
var qwe = CompressedPackageLoader.IsSupportedCompressedPackage("");
var asd = CompressedPackageLoader.GetEntryPaths(@"C:\Users\Jim.Jiang\Downloads\RoomsHost-20230531_152036478_pid-21544.init.log.zip").ToArray();
//var asd1 = CompressedPackageLoader.GetEntryPaths(@"C:\Users\Jim.Jiang\Downloads\7zTest1.zip").ToArray();
//C:\Users\Jim.Jiang\Downloads\7zTest1.zip|7zTest1/Folder1.7z|Folder1\Folder11.zip|Folder11/Folder111.7z|Folder111\Folder1111.zip|Folder1111/Folder11111.7z|Folder11111\1.zip|1.txt
foreach (var path in asd)
{
    var s = CompressedPackageReader.ReadStream(path);
    StreamReader sReader = new StreamReader(s, Encoding.UTF8);
    var asaaaaad = sReader.ReadToEnd();
}
int i1 = 1;
IEnumerable<string> Lines(string log)
{
    string pattern = @"^\d{2}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}\.\d{3}";
    string previousLine = string.Empty;
    string currentLine;
    using (StreamReader reader = new StreamReader(log))
    {
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (Regex.IsMatch(currentLine, pattern))
            {
                if (!string.IsNullOrEmpty(previousLine))
                {
                    yield return previousLine;
                }
                previousLine = currentLine;
            }
            else
            {
                previousLine += Environment.NewLine + currentLine;
            }
        }

        if (!string.IsNullOrEmpty(previousLine))
        {
            yield return previousLine;
        }
    }
}
try
{
    var logFilePath = @"C:\Users\Jim.Jiang\Downloads\RoomsHost-20230529_131356617_pid-2152.init.log\2.log";
    Stopwatch stopwatch = Stopwatch.StartNew();
    var linesEnumerable = Lines(logFilePath);
    var lines = linesEnumerable.ToArray();
    var duration = stopwatch.ElapsedMilliseconds;
    Console.WriteLine("duration: " + duration);
    stopwatch.Stop();

    stopwatch.Restart();
    string pattern = @"^\d{2}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}\.\d{3}";
    string previousLine = string.Empty;
    string currentLine;
    List<string> linesList = new List<string>();
    using (StreamReader reader = new StreamReader(logFilePath))
    {
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (Regex.IsMatch(currentLine, pattern))
            {
                if (!string.IsNullOrEmpty(previousLine))
                {
                    linesList.Add(previousLine);
                    var r = "^(\\d{2}\\/\\d{2}\\/\\d{2} \\d{2}:\\d{2}:\\d{2}.\\d{3}) \\<(.*?)\\> \\[(.*?)\\] (.*?) (.*)";
                    var match = Regex.Match(previousLine, r, RegexOptions.Singleline);
                    var captureCells = match.Groups.Values.Skip(1);
                    var c = captureCells.Select(x => x.Value).ToArray();
                }
                previousLine = currentLine;
            }
            else
            {
                previousLine += Environment.NewLine + currentLine;
            }
        }

        if (!string.IsNullOrEmpty(previousLine))
        {
            linesList.Add(previousLine);
        }
    }
    duration = stopwatch.ElapsedMilliseconds;
    Console.WriteLine("duration: " + duration);
    stopwatch.Stop();
}
catch (Exception ex)
{
    Console.WriteLine("Error reading the file: " + ex.Message);
}
