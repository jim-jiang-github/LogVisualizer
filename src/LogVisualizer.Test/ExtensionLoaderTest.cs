using LogVisualizer.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class ExtensionLoaderTest
    {
        public ExtensionLoaderTest()
        {
        }

        //[Theory]
        //[InlineData(@"Samples\SevenZ\7zTest.7z", 12)]
        //public async Task LoadFromFolder7Z(string sevenZPath, int expected)
        //{
        //    var paths = ExtensionLoaderProvider.GetSourcePaths(sevenZPath, new Dictionary<string, ExtensionLoaderProvider.LogExtensionLoaderType>()
        //    {
        //        { ".7z", ExtensionLoaderProvider.LogExtensionLoaderType.SevenZ}
        //    });
        //    Assert.Equal(expected, paths.Count());
        //    var reader = LogReaderProvider.GetReader(LogReaderProvider.LogReaderType.SevenZ);
        //    foreach (var path in paths)
        //    {
        //        using var stream = reader?.Read(path);
        //        if (stream == null)
        //        {
        //            Assert.Fail("stream is null.");
        //        }
        //        StreamReader sr = new StreamReader(stream);
        //        var readContent = sr.ReadLine();
        //        var textContent = Path.GetFileNameWithoutExtension(path);
        //        Assert.Equal(readContent, textContent);
        //    }
        //}

        //[Theory]
        //[InlineData(@"Samples\Zip\7zTest.zip", 12)]
        //public async Task LoadFromFolderZip(string sevenZPath, int expected)
        //{
        //    var paths = ExtensionLoaderProvider.GetSourcePaths(sevenZPath, new Dictionary<string, ExtensionLoaderProvider.LogExtensionLoaderType>()
        //    {
        //        { ".zip", ExtensionLoaderProvider.LogExtensionLoaderType.Zip}
        //    });
        //    Assert.Equal(expected, paths.Count());
        //    var reader = LogReaderProvider.GetReader(LogReaderProvider.LogReaderType.Zip);
        //    foreach (var path in paths)
        //    {
        //        using var stream = reader?.Read(path);
        //        if (stream == null)
        //        {
        //            Assert.Fail("stream is null.");
        //        }
        //        StreamReader sr = new StreamReader(stream);
        //        var readContent = sr.ReadLine();
        //        var textContent = Path.GetFileNameWithoutExtension(path);
        //        Assert.Equal(readContent, textContent);
        //    }
        //}
    }
}
