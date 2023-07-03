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
using LogVisualizer.ViewModels;
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
        private readonly INotify _notify;
        private readonly DebounceDispatcher _debounceDispatcher;

        private List<LogFilterItem> _logFilterItems = new List<LogFilterItem>();

        public string[] SupportedLogExtension => CurrentScenario?.SupportedExtensions ?? new[] { "*.txt", "*.log" };
        public IEnumerable<Scenario> Scenarios { get; private set; } = Array.Empty<Scenario>();
        public Scenario? CurrentScenario { get; private set; }
        public IEnumerable<LogFileItem> LogFileItems { get; private set; }
        public IEnumerable<LogFilterItem> LogFilterItems => _logFilterItems;
        public LogFileItem? CurrentLogFileItem { get; private set; }

        public ScenarioService(INotify notify)
        {
            _notify = notify;
            _debounceDispatcher = new DebounceDispatcher();
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

        public Task OpenLogFileItems(IReadOnlyList<IStorageFile> storageFiles)
        {
            return Task.Run(() =>
            {
                Loading.SetMessage(I18NKeys.Loading_OpenFileStart.GetLocalizationRawValue());

                IEnumerable<string> filePaths = storageFiles
                    .Where(x => x.CanBookmark).Select(async x => await x.SaveBookmarkAsync())
                    .Select(x => x.Result)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .OfType<string>();
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
                    if (ArchiveLoader.IsSupportedArchive(x))
                    {
                        var subItems = ArchiveLoader.GetEntryPaths(x).Select(i => new LogFileItem(Path.GetFileName(i), i, null, null)).ToArray();
                        return new LogFileItem(Path.GetFileName(x), x, null, subItems);
                    }
                    else
                    {
                        return new LogFileItem(Path.GetFileName(x), x, null, null);
                    }
                }).ToArray();
            if (!logFileItems.Any())
            {
                return Array.Empty<LogFileItem>();
            }
            return logFileItems;
        }

        public async Task LoadLogFileItem(LogFileItem? logFileItem)
        {
            CurrentLogFileItem = logFileItem;
            if (CurrentLogFileItem == null ||
                CurrentLogFileItem.Path == null ||
                CurrentScenario == null)
            {
                return;
            }
            await Task.Run(() =>
            {
                var logContent = CurrentScenario.LoadLogContent(CurrentLogFileItem.Path);
                WeakReferenceMessenger.Default.Send(new LogContentSelectedChangedMessage(logContent));
            });
        }

        public async Task<LogFilterItem?> CreateFilterItem(string filterKey)
        {
            LogFilterItem logFilterItem = new()
            {
                FilterKey = filterKey,
            };
            var confirmButtonText = I18NKeys.Common_Confirm.GetLocalizationRawValue();
            var clickedButtonTest = await _notify.ShowSubWindow(
                I18NKeys.Filter_Dialog_Create.GetLocalizationRawValue(),
                new LogFilterItemEditorViewModel()
                {
                    LogFilterItem = logFilterItem
                },
                new[]
                {
                    new MessageBoxButton(confirmButtonText, true)
                });
            if (clickedButtonTest != confirmButtonText)
            {
                return null;
            }
            return logFilterItem;
        }

        public void AddFilterItem(LogFilterItem logFilterItem)
        {
            _logFilterItems.Add(logFilterItem);
            logFilterItem.PropertyChanged += LogFilterItem_PropertyChanged;
            FilterChanged();
        }

        public Task EditFilterItem(LogFilterItem logFilterItem)
        {
            return _notify.ShowSubWindow(
                I18NKeys.Filter_Dialog_Edit.GetLocalizationRawValue(),
                new LogFilterItemEditorViewModel()
                {
                    LogFilterItem = logFilterItem
                });
        }

        public async Task<bool> RemoveFilterItem(LogFilterItem logFilterItem)
        {
            var content = I18NKeys.Common_ConfirmDelete.GetLocalizationString(logFilterItem.FilterKey);
            if (await _notify.ShowComfirmMessageBox(content))
            {
                _logFilterItems.Remove(logFilterItem);
                logFilterItem.PropertyChanged -= LogFilterItem_PropertyChanged;
                WeakReferenceMessenger.Default.Send(new LogFilterItemsChangedMessage(_logFilterItems));
                return true;
            }
            return false;
        }

        private void LogFilterItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LogFilterItem.Id) ||
                e.PropertyName == nameof(LogFilterItem.HexColor) ||
                e.PropertyName == nameof(LogFilterItem.Hits))
            {

            }
            else
            {
                FilterChanged();
            }
        }

        private void FilterChanged()
        {
            _debounceDispatcher.Debounce(200, async (x) =>
            {
                _ = Task.Run(() =>
                {
                    WeakReferenceMessenger.Default.Send(new LogFilterItemsChangedMessage(LogFilterItems));
                });
            });
        }
    }
}
