using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using Avalonia;
using System.Collections;
using LogVisualizer.Services;
using LogVisualizer.Views;
using LogVisualizer.Scenarios;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Models;
using LogVisualizer.Scenarios.Contents;
using System.Text.RegularExpressions;
using Serilog.Context;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.Commons;
using LogVisualizer.I18N;

namespace LogVisualizer.ViewModels
{
    public partial class LogViewerViewModel : ViewModelBase
    {
        private INotify _notify;
        private readonly FilterService _filterService;
        private LogProcessorService _logProcessorService;
        private string[] _columnNames;
        private int _mainColumnIndex = 0;

        [ObservableProperty]
        private LogRow? _selectedRow = null;
        [ObservableProperty]
        private ObservableCollection<LogRow> _displayRows;
        [ObservableProperty]
        private bool _showOnlyFilteredLines;

        public LogViewerViewModel(INotify notify, FilterService filterService, LogProcessorService logProcessorService)
        {
            _notify = notify;
            _filterService = filterService;
            _logProcessorService = logProcessorService;
            _showOnlyFilteredLines = _logProcessorService.ShowOnlyFilteredLines;
            WeakReferenceMessenger.Default.Register<LogDisplayRowsChangedMessage>(this, (r, m) =>
            {
                _columnNames = m.ColumnNames;
                _mainColumnIndex = m.MainColumnIndex;
                var displayRows = m.Value;
                DisplayRows = new ObservableCollection<LogRow>(displayRows);
            });
        }

        partial void OnShowOnlyFilteredLinesChanged(bool value)
        {
            _logProcessorService.ShowOnlyFilteredLines = value;
        }

        [RelayCommand]
        private void DisplayModeChanged()
        {
            ShowOnlyFilteredLines = !ShowOnlyFilteredLines;
        }

        [RelayCommand]
        private async Task ShowLogRowDetail()
        {
            if (SelectedRow == null)
            {
                return;
            }
            var logRowDetailViewModel = DependencyInjectionProvider.GetService<LogRowDetailViewModel>();
            if (logRowDetailViewModel == null)
            {
                return;
            }
            logRowDetailViewModel.SetLogRow(_columnNames.Select(c => $"{c}:").ToArray(), SelectedRow.Value);
            await _notify.ShowSubWindow(I18NKeys.LogViewer_Dialog_Title.GetLocalizationRawValue(), logRowDetailViewModel);
        }

        [RelayCommand]
        private async Task CreateLogFilterItem()
        {
            if (SelectedRow == null)
            {
                return;
            }
            LogRow selectedRiow = SelectedRow.Value;
            var mainCell = selectedRiow.Cells[_mainColumnIndex];
            if (mainCell?.ToString() is not string mainCellStr)
            {
                return;
            }
            await _filterService.CreateFilterItem(mainCellStr);
        }
    }
}