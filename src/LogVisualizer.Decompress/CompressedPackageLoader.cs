using SevenZipExtractor;
using System.IO.Compression;
using System.Reflection;

namespace LogVisualizer.Decompress
{
    public abstract class CompressedPackageLoader
    {
        #region InnerClass
        private class CompressedPackageLoaderZip : CompressedPackageLoader
        {
            protected override string Extension => ".zip";

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
        private class CompressedPackageLoader7Z : CompressedPackageLoader
        {
            protected override string Extension => ".7z";

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

        private static CompressedPackageLoader[] AllCompressedPackageLoaders { get; }
        static CompressedPackageLoader()
        {
            AllCompressedPackageLoaders = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(CompressedPackageLoader)))
                .Select(x => Activator.CreateInstance(x) as CompressedPackageLoader)
                .OfType<CompressedPackageLoader>()
                .ToArray();
        }
        private static IEnumerable<string> GetEntryPaths(EntryItem entryItem)
        {
            var extension = Path.GetExtension(entryItem.EntryPath);

            CompressedPackageLoader? compressedPackageLoader = AllCompressedPackageLoaders.FirstOrDefault(x => x.Extension == extension);
            if (compressedPackageLoader == null)
            {
                yield return entryItem.EntryPath;
            }
            else
            {
                var entryItems = compressedPackageLoader.GetEntryPathsInternal(entryItem).ToArray();
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

            CompressedPackageLoader? compressedPackageLoader = AllCompressedPackageLoaders.FirstOrDefault(x => x.Extension == extension);
            if (compressedPackageLoader == null)
            {
                yield return entryPath;
            }
            else
            {
                using var entryItemStream = File.OpenRead(entryPath);
                var entryItems = compressedPackageLoader.GetEntryPathsInternal(new EntryItem(entryPath, entryItemStream)).ToArray();
                foreach (var entryItem in entryItems)
                {
                    foreach (var path in GetEntryPaths(entryItem))
                    {
                        yield return path;
                    }
                }
            }
        }
        public static bool IsSupportedCompressedPackage(string entryPath)
        {
            var extension = Path.GetExtension(entryPath);
            CompressedPackageLoader? compressedPackageLoader = AllCompressedPackageLoaders.FirstOrDefault(x => x.Extension == extension);
            return compressedPackageLoader != null;
        }

        private CompressedPackageLoader() { }
        protected abstract string Extension { get; }
        internal abstract IEnumerable<EntryItem> GetEntryPathsInternal(EntryItem entryItem);
    }
}