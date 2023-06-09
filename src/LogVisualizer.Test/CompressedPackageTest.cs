using LogVisualizer.Decompress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class CompressedPackageTest
    {
        public CompressedPackageTest()
        {
        }

        [Theory]
        [InlineData(@"Samples\CompressedPackage\7zTest.7z", 12)]
        [InlineData(@"Samples\CompressedPackage\7zTest.zip", 12)]
        [InlineData(@"Samples\CompressedPackage\7zTest1.7z", 2)]
        [InlineData(@"Samples\CompressedPackage\7zTest1.zip", 2)]
        public async Task LoadCompressedPackage(string compressedPackagePath, int expected)
        {
            var paths = CompressedPackageLoader.GetEntryPaths(compressedPackagePath);
            int count = paths.Count();
            Assert.Equal(expected, count);
        }

        [Theory]
        [InlineData(@"Samples\CompressedPackage\7zTest.7z")]
        [InlineData(@"Samples\CompressedPackage\7zTest.zip")]
        [InlineData(@"Samples\CompressedPackage\7zTest1.7z")]
        [InlineData(@"Samples\CompressedPackage\7zTest1.zip")]
        public async Task ReadCompressedPackage(string compressedPackagePath)
        {
            var paths = CompressedPackageLoader.GetEntryPaths(compressedPackagePath);
            foreach (string path in paths)
            {
                using var stream = CompressedPackageReader.ReadStream(path);
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
        //}
    }
}
