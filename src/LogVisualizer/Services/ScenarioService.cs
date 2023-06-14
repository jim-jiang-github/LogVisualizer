using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.Commons;
using LogVisualizer.Decompress;
using LogVisualizer.I18N;
using LogVisualizer.Messages;
using LogVisualizer.Models;
using LogVisualizer.Scenarios;
using LogVisualizer.Scenarios.Contents;
using LogVisualizer.Scenarios.Schemas;
using Metalama.Framework.Code.Collections;
using Serilog.Context;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class ScenarioService
    {
        public string[] SupportedLogExtension => CurrentScenario?.SupportedExtensions ?? new[] { "*.txt", "*.log" };
        public IEnumerable<Scenario> Scenarios { get; private set; } = Array.Empty<Scenario>();
        public Scenario? CurrentScenario { get; private set; }
        public IEnumerable<LogFileItem> LogFileItems { get; private set; }
        public LogFileItem? CurrentLogFileItem { get; private set; }

        public ScenarioService()
        {
            ReloadScenarios();
        }

        public void ReloadScenarios()
        {
            if (!Directory.Exists(Global.ScenarioConfigFolderRoot))
            {
                CurrentScenario = null;
                Scenarios = Array.Empty<Scenario>();
                return;
            }
            var scenarios = Directory.GetDirectories(Global.ScenarioConfigFolderRoot)
                .Select(directory => Scenario.LoadFromFolder(directory))
                .OfType<Scenario>()
                .ToArray();
            Scenarios = scenarios;
            if (scenarios.Length == 1)
            {
                CurrentScenario = Scenarios.FirstOrDefault();
                if (CurrentScenario != null && Configuration.Instance.DefaultScenario != CurrentScenario.Name)
                {
                    Configuration.Instance.DefaultScenario = CurrentScenario.Name;
                    Configuration.Instance.Save();
                    return;
                }
            }
            else
            {
                CurrentScenario = Scenarios.FirstOrDefault(s => s.Name == Configuration.Instance.DefaultScenario);
            }
        }

        public Task OpenAndSelectLogFileItems()
        {
            return Task.Run(async () =>
            {
                Loading.SetMessage(I18NKeys.Loading_OpenFileStart.GetLocalizationRawValue());
                var supportedFileTypes = new FilePickerFileType[]
                {
                new(I18NKeys.OpenFileDialog_SupportedLogs.GetLocalizationRawValue())
                {
                    Patterns = SupportedLogExtension.Concat(CompressedPackageLoader.SupportedExtensions).ToArray()
                }
                };
                var storageFiles = await GlobalStorageProvider.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = I18NKeys.OpenFileDialog_PickLog.GetLocalizationRawValue(),
                    FileTypeFilter = supportedFileTypes,
                    AllowMultiple = true
                });
                if (storageFiles.Count == 0)
                {
                    return;
                }
                IEnumerable<string> filePaths = storageFiles
                    .Where(x => x.CanBookmark).Select(async x => await x.SaveBookmarkAsync())
                    .Select(x => x.Result)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .WhereNotNull();
                var logFileItems = LoadLogFileItems(filePaths);
                LogFileItems = logFileItems;
                WeakReferenceMessenger.Default.Send(new LogFileItemsChangedMessage(LogFileItems));
                Loading.SetProgress(1);
            });
        }

        private IEnumerable<LogFileItem> LoadLogFileItems(IEnumerable<string> filePaths)
        {
            var logFileItems = filePaths
                .Select(x =>
                {
                    Loading.SetMessage(I18NKeys.Loading_LoadingFile.GetLocalizationString(x));
                    if (CompressedPackageLoader.IsSupportedCompressedPackage(x))
                    {
                        var subItems = CompressedPackageLoader.GetEntryPaths(x).Select(i => new LogFileItem(Path.GetFileName(i), i, null, null)).ToArray();
                        return new LogFileItem(Path.GetFileName(x), x, null, subItems);
                    }
                    else
                    {
                        return new LogFileItem(Path.GetFileName(x), x, null, null);
                    }
                });
            if (!logFileItems.Any())
            {
                return Array.Empty<LogFileItem>();
            }
            return logFileItems;
        }

        public void LoadLogFileItem(LogFileItem? logFileItem)
        {
            CurrentLogFileItem = logFileItem;
            if (CurrentLogFileItem == null)
            {
                return;
            }
            if (CompressedPackageLoader.IsSupportedCompressedPackage(CurrentLogFileItem.Path))
            {
                var stream = CompressedPackageReader.ReadStream(CurrentLogFileItem.Path);
                var logContent = ILogContent.LoadLogContent(stream, CurrentScenario.SchemaLogPath);
            }
            WeakReferenceMessenger.Default.Send(new LogFileItemSelectedChangedMessage(logFileItem));
        }
    }
}
