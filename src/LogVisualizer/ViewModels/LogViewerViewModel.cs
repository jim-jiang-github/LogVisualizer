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

namespace LogVisualizer.ViewModels
{
    public partial class LogViewerViewModel : ViewModelBase
    {
        private LogFilterViewModel _logFilterViewModel;
        private IEnumerable<LogRow> currentRows;
        [ObservableProperty]
        private RangeObservableCollection<LogRow> _items;

        public LogViewerViewModel(LogFilterViewModel logFilterViewModel)
        {
            _logFilterViewModel = logFilterViewModel;
            Items = new RangeObservableCollection<LogRow>();
            WeakReferenceMessenger.Default.Register<LogContentSelectedChangedMessage>(this, (r, m) =>
            {
                var logContent = m.Value;
                if (logContent == null)
                {
                    Items.Clear();
                    return;
                }
                currentRows = logContent.Rows;
                Items.Clear();
                Items.AddRange(currentRows);
            });
            WeakReferenceMessenger.Default.Register<LogFilterItemsChangedMessage>(this, (r, m) =>
            {
                Items.Clear();
                var result = currentRows.Where(row =>
                {
                    bool matched = m.Value.Where(f => f.Enabled).All(f => Search(row.Cells[4].ToString(), f.FilterKey, f.IsMatchCase, f.IsMatchWholeWord, f.IsUseRegularExpression));
                    return matched;
                });
                Items.AddRange(result);
            });
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
    }

}