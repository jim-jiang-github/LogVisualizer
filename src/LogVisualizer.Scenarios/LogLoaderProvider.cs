using System.IO.MemoryMappedFiles;

namespace LogVisualizer.Scenarios
{
    internal class LogLoaderProvider
    {
        #region InnerClass
        internal enum LogLoaderType
        {
            Unknow,
            Txt,
            MemoryMapped,
        }
        internal abstract class LogLoader
        {
            public abstract Stream Load(string filePath);
        }
        private class StreamLoaderMemoryMapped : LogLoader
        {
            public override Stream Load(string filePath)
            {
                using MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(filePath);
                var stream = memoryMappedFile.CreateViewStream();
                return stream;
            }
        }
        private class StreamLoaderText : LogLoader
        {
            public override Stream Load(string filePath)
            {
                var stream = File.OpenRead(filePath);
                return stream;
            }
        }
        #endregion
        public static LogLoader? GetLoader(LogLoaderType loaderType)
        {
            switch (loaderType)
            {
                case LogLoaderType.Txt:
                    return new StreamLoaderText();
                case LogLoaderType.MemoryMapped:
                    return new StreamLoaderMemoryMapped();
                default:
                    return null;
            }
        }
    }
}
