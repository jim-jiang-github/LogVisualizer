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

namespace LogVisualizer.ViewModels
{
    public partial class LogViewerViewModel : ViewModelBase
    {
        private LogFilterViewModel _logFilterViewModel;
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
                Items.Clear();
                Items.AddRange(logContent.Rows);
            });
        }
    }

}