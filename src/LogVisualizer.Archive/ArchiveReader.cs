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
    public abstract class ArchiveReader
    {
        #region InnerClass
        private class ArchiveReaderZip : ArchiveReader
        {
            protected override string Extension => "zip";

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
        private class ArchiveReader7Z : ArchiveReader
        {
            protected override string Extension => "7z";

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

        private static ArchiveReader[] AllArchiveReaders { get; }
        static ArchiveReader()
        {
            AllArchiveReaders = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ArchiveReader)))
                .Select(x => Activator.CreateInstance(x) as ArchiveReader)
                .OfType<ArchiveReader>()
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
            ArchiveReader? archiveReader = AllArchiveReaders.FirstOrDefault(x => $".{x.Extension}" == extension);
            if (archiveReader == null)
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
                var stream = archiveReader.ReadStreamInternal(entryItem);
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
        private ArchiveReader() { }
        protected abstract string Extension { get; }
        internal abstract Stream? ReadStreamInternal(EntryItem entryItem);
    }
}
