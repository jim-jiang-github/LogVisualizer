using LogVisualizer.Decompress;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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
