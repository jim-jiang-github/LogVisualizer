using SevenZipExtractor;
using System.IO.Compression;
using System.Reflection;

namespace LogVisualizer.Decompress
{
    public abstract class ArchiveLoader
    {
        #region InnerClass
        private class ArchiveLoaderZip : ArchiveLoader
        {
            protected override string Extension => "zip";

            internal override IEnumerable<EntryItem> GetEntryPathsInternal(EntryItem entryItem)
            {
                using (Stream entryItemStream = entryItem.EntryItemStream)
                using (ZipArchive archiveFile = new ZipArchive(entryItemStream))
                {
                    foreach (EntryItem outputEntryItem in archiveFile.Entries
                        .Where(x => !x.FullName.EndsWith("/"))
                        .Select(x =>
                        {
                            MemoryStream stream = new();
                            using var openStream = x.Open();
                            openStream.CopyTo(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            return new EntryItem($"{entryItem.EntryPath}|{x.FullName}", stream);
                        }))
                    {
                        yield return outputEntryItem;
                    }
                }
            }
        }
        private class ArchiveLoader7Z : ArchiveLoader
        {
            protected override string Extension => "7z";

            internal override IEnumerable<EntryItem> GetEntryPathsInternal(EntryItem entryItem)
            {
                using (Stream entryItemStream = entryItem.EntryItemStream)
                using (ArchiveFile archiveFile = new ArchiveFile(entryItemStream))
                {
                    foreach (EntryItem outputEntryItem in archiveFile.Entries
                        .Where(x => !x.IsFolder)
                        .Select(x =>
                        {
                            MemoryStream stream = new();
                            x.Extract(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            return new EntryItem($"{entryItem.EntryPath}|{x.FileName}", stream);
                        }))
                    {
                        yield return outputEntryItem;
                    }
                }
            }
        }
        #endregion

        private static ArchiveLoader[] AllArchiveLoaders { get; }
        public static string[] SupportedExtensions => AllArchiveLoaders.Select(x => $"*.{x.Extension}").ToArray();
        static ArchiveLoader()
        {
            AllArchiveLoaders = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ArchiveLoader)))
                .Select(x => Activator.CreateInstance(x) as ArchiveLoader)
                .OfType<ArchiveLoader>()
                .ToArray();
        }
        private static IEnumerable<string> GetEntryPaths(EntryItem entryItem)
        {
            var extension = Path.GetExtension(entryItem.EntryPath);

            ArchiveLoader? archiveLoader = AllArchiveLoaders.FirstOrDefault(x => $".{x.Extension}" == extension);
            if (archiveLoader == null)
            {
                yield return entryItem.EntryPath;
            }
            else
            {
                var entryItems = archiveLoader.GetEntryPathsInternal(entryItem).ToArray();
                foreach (EntryItem item in entryItems)
                {
                    foreach (var path in GetEntryPaths(item))
                    {
                        yield return path;
                    }
                }
            }
        }
        public static IEnumerable<string> GetEntryPaths(string entryPath)
        {
            var extension = Path.GetExtension(entryPath);

            ArchiveLoader? archiveLoader = AllArchiveLoaders.FirstOrDefault(x => $".{x.Extension}" == extension);
            if (archiveLoader == null)
            {
                yield return entryPath;
            }
            else
            {
                using var entryItemStream = File.OpenRead(entryPath);
                var entryItems = archiveLoader.GetEntryPathsInternal(new EntryItem(entryPath, entryItemStream)).ToArray();
                foreach (var entryItem in entryItems)
                {
                    foreach (var path in GetEntryPaths(entryItem))
                    {
                        yield return path;
                    }
                }
            }
        }
        public static bool IsArchiveEntry(string entryPath)
        {
            int delimiterIndex = entryPath.IndexOf("|");
            if (delimiterIndex == -1)
            {
                return false;
            }
            entryPath = entryPath.Substring(0, delimiterIndex);
            var extension = Path.GetExtension(entryPath);
            bool isArchiveEntry = SupportedExtensions.Any(x => x == $"*{extension}");
            return isArchiveEntry;
        }
        public static bool IsSupportedArchive(string entryPath)
        {
            var extension = Path.GetExtension(entryPath);
            bool isSupported = SupportedExtensions.Any(x => x == $"*{extension}");
            return isSupported;
        }

        private ArchiveLoader() { }
        protected abstract string Extension { get; }
        internal abstract IEnumerable<EntryItem> GetEntryPathsInternal(EntryItem entryItem);
    }
}