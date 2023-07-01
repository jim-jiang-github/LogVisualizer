using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using System.Collections;
using LogVisualizer.Services;
using LogVisualizer.Views;
using System.Collections.ObjectModel;
using LogVisualizer.Models;
using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.Commons.Attributes;
using LogVisualizer.Commons;
using System.Threading;
using Newtonsoft.Json.Linq;
using Avalonia.Media;
using LogVisualizer.I18N;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;

namespace LogVisualizer.ViewModels
{
    public partial class LogFilterViewModel : ViewModelBase
    {
        private ScenarioService _scenarioService;

        [ObservableProperty]
        private LogFilterItem? _selectedItem = null;
        private IEnumerable<LogFilterItem> LogFilterItems => _scenarioService.LogFilterItems;

        public LogFilterViewModel(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
            WeakReferenceMessenger.Default.Register<LogFilterItemsChangedMessage>(this, (r, m) =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    OnPropertyChanged(nameof(LogFilterItems));
                });
            });
        }

        partial void OnSelectedItemChanged(LogFilterItem? oldValue, LogFilterItem? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= Value_PropertyChanged;
            }
            if (newValue != null)
            {
                newValue.PropertyChanged += Value_PropertyChanged;
            }
        }

        private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _scenarioService.FilterChanged(LogFilterItems);
        }

        private void LogFilterItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _scenarioService.FilterChanged(LogFilterItems);
        }

        [RelayCommand]
        private async Task CreateLogFilterItem()
        {
            LogFilterItem? logFilterItem = await _scenarioService.CreateFilterItem(string.Empty);
            if (logFilterItem == null)
            {
                return;
            }
            _scenarioService.AddFilterItem(logFilterItem);
            SelectedItem = logFilterItem;
        }

        [RelayCommand]
        private void EditLogFilterItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            _scenarioService.EditFilterItem(SelectedItem);
        }

        [RelayCommand]
        private async Task RemoveSelectedLogFilterItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            await RemoveLogFilterItem(SelectedItem);
        }

        [RelayCommand]
        private async Task RemoveLogFilterItem(LogFilterItem logFilterItem)
        {
            var content = I18NKeys.Common_ConfirmDelete.GetLocalizationString(logFilterItem.FilterKey);
            if (await Notify.ShowComfirmMessageBox(content))
            {
                _scenarioService.RemoveFilterItem(logFilterItem);
            }
        }
    }
}