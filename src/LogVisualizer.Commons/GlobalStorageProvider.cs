using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public class GlobalStorageProvider
    {
        private class IStorageProviderDefault : IStorageProvider
        {
            public bool CanOpen => false;

            public bool CanSave => false;

            public bool CanPickFolder => false;

            public Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark)
            {
                return Task.FromResult<IStorageBookmarkFile?>(null);
            }

            public Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
            {
                return Task.FromResult<IReadOnlyList<IStorageFile>>(Array.Empty<IStorageFile>());
            }

            public Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark)
            {
                return Task.FromResult<IStorageBookmarkFolder?>(null);
            }

            public Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
            {
                return Task.FromResult<IReadOnlyList<IStorageFolder>>(Array.Empty<IStorageFolder>());
            }

            public Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
            {
                return Task.FromResult<IStorageFile?>(null);
            }

            public Task<IStorageFile?> TryGetFileFromPathAsync(Uri filePath)
            {
                return Task.FromResult<IStorageFile?>(null);
            }

            public Task<IStorageFolder?> TryGetFolderFromPathAsync(Uri folderPath)
            {
                return Task.FromResult<IStorageFolder?>(null);
            }

            public Task<IStorageFolder?> TryGetWellKnownFolderAsync(WellKnownFolder wellKnownFolder)
            {
                return Task.FromResult<IStorageFolder?>(null);
            }
        }

        public static IStorageProvider StorageProvider { get; set; } = new IStorageProviderDefault();
    }
}
