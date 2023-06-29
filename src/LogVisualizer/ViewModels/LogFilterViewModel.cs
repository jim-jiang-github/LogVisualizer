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

namespace LogVisualizer.ViewModels
{
    public partial class LogFilterViewModel : ViewModelBase
    {
        private readonly DebounceDispatcher _debounceDispatcher;
        private ScenarioService _scenarioService;

        [ObservableProperty]
        private LogFilterItem? _selectedItem = null;
        [ObservableProperty]
        private ObservableCollection<LogFilterItem> _logFilterItems;

        public LogFilterViewModel(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
            _debounceDispatcher = new DebounceDispatcher();
            LogFilterItems = new ObservableCollection<LogFilterItem>();
            LogFilterItems.CollectionChanged += LogFilterItems_CollectionChanged;
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
            NotifyAnyLogFilterChanged();
        }

        private void LogFilterItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyAnyLogFilterChanged();
        }

        private void NotifyAnyLogFilterChanged() 
        {
            _debounceDispatcher.Debounce(400, async (x) =>
            {
                WeakReferenceMessenger.Default.Send(new LogFilterItemsChangedMessage(LogFilterItems));
            });
        }

        [RelayCommand]
        private async Task AddLogFilterItem()
        {
            LogFilterItem logFilterItem = new LogFilterItem();
            LogFilterItems.Add(logFilterItem);
            LogFilterItemDetailSelectedChangedMessage logFilterItemDetailSelectedChangedMessage = new LogFilterItemDetailSelectedChangedMessage(logFilterItem);
            WeakReferenceMessenger.Default.Send(logFilterItemDetailSelectedChangedMessage);
            bool success = await logFilterItemDetailSelectedChangedMessage.Response;
            if (!success)
            {
                LogFilterItems.Remove(logFilterItem);
            }
        }

        [RelayCommand]
        private void EditLogFilterItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            WeakReferenceMessenger.Default.Send(new LogFilterItemDetailSelectedChangedMessage(SelectedItem));
        }

        [RelayCommand]
        private async Task DeleteSelectedLogFilterItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            await DeleteLogFilterItem(SelectedItem);
        }

        [RelayCommand]
        private async Task DeleteLogFilterItem(LogFilterItem logFilterItem)
        {
            var content = I18NKeys.Common_ConfirmDelete.GetLocalizationString(logFilterItem.FilterKey);
            if (await Notify.ShowComfirmMessageBox(content))
            {
                LogFilterItems.Remove(logFilterItem);
            }
        }
    }
}