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
        private readonly FilterService _filterService;

        [ObservableProperty]
        private LogFilterItem? _selectedItem = null;
        [ObservableProperty]
        private ObservableCollection<LogFilterItem> _logFilterItems;

        public LogFilterViewModel(FilterService filterService)
        {
            _filterService = filterService;
            _logFilterItems = new ObservableCollection<LogFilterItem>();
            WeakReferenceMessenger.Default.Register<LogFilterItemsChangedMessage>(this, (r, m) =>
            {
                LogFilterItems = new ObservableCollection<LogFilterItem>(_filterService.LogFilterItems);
            });
        }

        [RelayCommand]
        private async Task CreateLogFilterItem()
        {
            await _filterService.CreateFilterItem(string.Empty);
        }

        [RelayCommand]
        private async Task EditLogFilterItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            await _filterService.EditFilterItem(SelectedItem);
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
            await _filterService.RemoveFilterItem(logFilterItem);
        }
    }
}