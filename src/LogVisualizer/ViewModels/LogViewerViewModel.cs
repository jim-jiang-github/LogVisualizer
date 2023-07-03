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

namespace LogVisualizer.ViewModels
{
    public partial class LogViewerViewModel : ViewModelBase
    {
        private LogFilterViewModel _logFilterViewModel;
        private ScenarioService _scenarioService;
        private IEnumerable<LogRow> _currentRows;
        private int _mainColumnIndex = 0;

        [ObservableProperty]
        private LogRow? _selectedRow = null;

        [ObservableProperty]
        private RangeObservableCollection<LogRow> _items;

        public LogViewerViewModel(LogFilterViewModel logFilterViewModel, ScenarioService scenarioService)
        {
            _logFilterViewModel = logFilterViewModel;
            _scenarioService = scenarioService;
            Items = new RangeObservableCollection<LogRow>();
            WeakReferenceMessenger.Default.Register<LogContentSelectedChangedMessage>(this, (r, m) =>
            {
                var logContent = m.Value;
                if (logContent == null)
                {
                    Items.Clear();
                    return;
                }
                _currentRows = logContent.Rows;
                _mainColumnIndex = logContent.MainColumnIndex;
                ApplyFilter();
            });
            WeakReferenceMessenger.Default.Register<LogFilterItemsChangedMessage>(this, (r, m) =>
            {
                ApplyFilter();
            });
        }

        private void ApplyFilter()
        {
            if (_currentRows == null)
            {
                return;
            }
            Items.Clear();
            var result = _currentRows.Where(row =>
            {
                bool matched = _scenarioService.LogFilterItems.Where(f => f.Enabled).All(f => Search(row.Cells[4].ToString(), f.FilterKey, f.IsMatchCase, f.IsMatchWholeWord, f.IsUseRegularExpression));
                return matched;
            });
            Items.AddRange(result);
        }

        private bool Search(string text, string keyword, bool matchCase, bool matchWholeWord, bool useRegex)
        {
            string pattern = keyword;
            if (!useRegex)
            {
                pattern = matchWholeWord ? $@"\b{Regex.Escape(keyword)}\b" : Regex.Escape(keyword);
            }

            return Regex.IsMatch(text, pattern, (matchCase | useRegex) ? RegexOptions.None : RegexOptions.IgnoreCase);
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
            LogFilterItem? logFilterItem = await _scenarioService.CreateFilterItem(mainCellStr);
            if (logFilterItem == null)
            {
                return;
            }
            _scenarioService.AddFilterItem(logFilterItem);
        }
    }
}