using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Decompress
{
    public abstract class CompressedPackageReader
    {
        #region InnerClass
        private class CompressedPackageReaderZip : CompressedPackageReader
        {
            protected override string Extension => ".zip";

            internal override Stream? ReadStreamInternal(EntryItem entryItem)
            {
                using (Stream entryItemStream = entryItem.EntryItemStream)
                using (ZipArchive archiveFile = new ZipArchive(entryItemStream))
                {
                    var entry = archiveFile.Entries
                        .Where(x => !x.FullName.EndsWith("/"))
                        .FirstOrDefault(x => x.FullName == entryItem.EntryPath);
                    if (entry == null)
                    {
                        return null;
                    }
                    MemoryStream stream = new();
                    using var openStream = entry.Open();
                    openStream.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return stream;
                }
            }
        }
        private class CompressedPackageReader7Z : CompressedPackageReader
        {
            protected override string Extension => ".7z";

            internal override Stream? ReadStreamInternal(EntryItem entryItem)
            {
                using (Stream entryItemStream = entryItem.EntryItemStream)
                using (ArchiveFile archiveFile = new ArchiveFile(entryItemStream))
                {
                    var entry = archiveFile.Entries
                        .Where(x => !x.IsFolder)
                        .FirstOrDefault(x => x.FileName == entryItem.EntryPath);
                    if (entry == null)
                    {
                        return null;
                    }
                    MemoryStream stream = new();
                    entry.Extract(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return stream;
                }
            }
        }
        #endregion

        private static CompressedPackageReader[] AllCompressedPackageReaders { get; }
        static CompressedPackageReader()
        {
            AllCompressedPackageReaders = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(CompressedPackageReader)))
                .Select(x => Activator.CreateInstance(x) as CompressedPackageReader)
                .OfType<CompressedPackageReader>()
                .ToArray();
        }
        public static Stream? ReadStream(string entryPath)
        {
            int delimiterIndex = entryPath.IndexOf("|");
            if (delimiterIndex == -1)
            {
                return null;
            }
            var currentPath = entryPath.Substring(0, delimiterIndex);
            var lastPath = entryPath.Substring(delimiterIndex + 1);
            using var entryItemStream = File.OpenRead(currentPath);
            return ReadStream(currentPath, entryItemStream, lastPath);
        }
        private static Stream? ReadStream(string currentPath, Stream entryItemStream, string? lastPath)
        {
            var extension = Path.GetExtension(currentPath);
            CompressedPackageReader? compressedPackageReader = AllCompressedPackageReaders.FirstOrDefault(x => x.Extension == extension);
            if (compressedPackageReader == null)
            {
                return entryItemStream;
            }
            else
            {
                int delimiterIndex = lastPath.IndexOf("|");
                if (delimiterIndex == -1)
                {
                    currentPath = lastPath;
                    lastPath = null;
                }
                else
                {
                    currentPath = lastPath.Substring(0, delimiterIndex);
                    lastPath = lastPath.Substring(delimiterIndex + 1);
                }
                var entryItem = new EntryItem(currentPath, entryItemStream);
                var stream = compressedPackageReader.ReadStreamInternal(entryItem);
                if (stream == null)
                {
                    return null;
                }
                if (lastPath == null)
                {
                    return stream;
                }
                return ReadStream(currentPath, stream, lastPath);
            }
        }
        private CompressedPackageReader() { }
        protected abstract string Extension { get; }
        internal abstract Stream? ReadStreamInternal(EntryItem entryItem);
    }
}
