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

namespace LogVisualizer.ViewModels
{
    public partial class LogSelectorViewModel : ViewModelBase
    {
        private ScenarioService _scenarioService;

        [ObservableProperty]
        private LogFileItem? _selectedItem = null;
        [ObservableProperty]
        private ObservableCollection<LogFileItem> _logFileItems;

        partial void OnSelectedItemChanged(LogFileItem? value)
        {
            if (value == null)
            {
                return;
            }
            if (value.Children != null)
            {
                return;
            }
            _scenarioService.LoadLogFileItem(value);
        }

        public LogSelectorViewModel(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
            LogFileItems = new ObservableCollection<LogFileItem>();
            WeakReferenceMessenger.Default.Register<LogFileItemsChangedMessage>(this, (r, m) =>
            {
                foreach (var item in m.Value)
                {
                    LogFileItems.Add(item);
                }
                if (LogFileItems.Count == 1)
                {
                    SelectedItem = LogFileItems[0];
                }
            });
        }
    }
}