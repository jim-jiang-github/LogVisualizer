using LogVisualizer.Decompress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class ArchiveTest
    {
        public ArchiveTest()
        {
        }

        [Theory]
        
        [InlineData(@"Samples/Archives/7zTest.7z", 12)]
        [InlineData(@"Samples/Archives/7zTest.zip", 12)]
        [InlineData(@"Samples/Archives/7zTest1.7z", 2)]
        [InlineData(@"Samples/Archives/7zTest1.zip", 2)]
        public async Task LoadArchives(string archivePath, int expected)
        {
            var paths = ArchiveLoader.GetEntryPaths(archivePath);
            int count = paths.Count();
            Assert.Equal(expected, count);
        }

        [Theory]
        [InlineData(@"Samples/Archives/7zTest.7z")]
        [InlineData(@"Samples/Archives/7zTest.zip")]
        [InlineData(@"Samples/Archives/7zTest1.7z")]
        [InlineData(@"Samples/Archives/7zTest1.zip")]
        public async Task ReadArchives(string archivePath)
        {
            var paths = ArchiveLoader.GetEntryPaths(archivePath);
            foreach (string path in paths)
            {
                using var stream = ArchiveReader.ReadStream(path);
                StreamReader reader = new StreamReader(stream);
                var newPath = path;
                int delimiterIndex = path.LastIndexOf("|");
                if (delimiterIndex != -1)
                {
                    newPath = newPath.Substring(delimiterIndex + 1);
                }
                var fileName = Path.GetFileNameWithoutExtension(newPath);
                var content = reader.ReadLine();
                Assert.Equal(fileName, content);
            }
        }

        [Theory]
        [InlineData(@"Samples/Archives/7zTest.7z", false)]
        [InlineData(@"Samples/Archives/7zTest.zip", false)]
        [InlineData(@"Samples/Archives/7zTest.zip|7zTest/Folder1/1.txt", true)]
        public void IsArchivesEntry(string archivePath, bool expected)
        {
            var result = ArchiveLoader.IsArchiveEntry(archivePath);
            Assert.Equal(expected, result);
        }
    }
}
