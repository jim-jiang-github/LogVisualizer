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
    public partial class LogFilterViewModel : ViewModelBase
    {
        private ScenarioService _scenarioService;

        [ObservableProperty]
        private LogFilterItemViewModel? _selectedItem = null;
        [ObservableProperty]
        private ObservableCollection<LogFilterItemViewModel> _logFilterItems;

        partial void OnSelectedItemChanged(LogFilterItemViewModel? value)
        {
            if (value == null)
            {
                return;
            }
        }

        public LogFilterViewModel(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
            _logFilterItems = new ObservableCollection<LogFilterItemViewModel>();
            for (int i = 0; i < 10; i++)
            {
                _logFilterItems.Add(new LogFilterItemViewModel()
                {
                    FilterKey = $"asdasd{i}"
                });
            }
        }
    }
}