using System.Diagnostics;
using System.Text.RegularExpressions;

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
    var lines = linesEnumerable.AsParallel()
                .AsOrdered()
                .Select((x) =>
                {
                    var r = "^(\\d{2}\\/\\d{2}\\/\\d{2} \\d{2}:\\d{2}:\\d{2}.\\d{3}) \\<(.*?)\\> \\[(.*?)\\] (.*?) (.*)";
                    var match = Regex.Match(x, r, RegexOptions.Singleline);
                    var captureCells = match.Groups.Values.Skip(1);
                    return captureCells.Select(x => x.Value).ToArray();
                });
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
                    var c= captureCells.Select(x => x.Value).ToArray();
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
