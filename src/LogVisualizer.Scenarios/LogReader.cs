using Newtonsoft.Json;
using System.IO.MemoryMappedFiles;
using System.IO;
using Avalonia.Controls.Shapes;

namespace LogVisualizer.Scenarios
{
    public abstract class LogReader
    {
        #region InnerClass
        public enum LogReaderType
        {
            None,
            Text,
            MemoryMapped,
        }
        private class StreamReaderMemoryMapped : LogReader
        {
            public override Stream? Read(string filePath)
            {
                try
                {
                    using MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(filePath);
                    var stream = memoryMappedFile.CreateViewStream();
                    return stream;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex.Message);
                    return null;
                }
            }
        }
        private class StreamReaderText : LogReader
        {
            public override Stream? Read(string filePath)
            {
                try
                {
                    var stream = File.OpenRead(filePath);
                    return stream;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex.Message);
                    return null;
                }
            }
        }
        #endregion
        public static LogReader? GetReader(LogReaderType type)
        {
            switch (type)
            {
                case LogReaderType.Text:
                    return new StreamReaderText();
                case LogReaderType.MemoryMapped:
                    return new StreamReaderMemoryMapped();
                default:
                    return null;
            }
        }
        public abstract Stream? Read(string filePath);
    }
}
