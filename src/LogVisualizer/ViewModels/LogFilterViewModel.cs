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

namespace LogVisualizer.ViewModels
{
    public partial class LogFilterViewModel : ViewModelBase
    {
        private ScenarioService _scenarioService;

        [ObservableProperty]
        private LogFilterItemViewModel? _selectedItem = null;
        [ObservableProperty]
        private ObservableCollection<LogFilterItemViewModel> _logFilterItems;

        [RelayCommand]
        private void EditLogFilterItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            WeakReferenceMessenger.Default.Send(new LogFilterItemDetailSelectedChangedMessage(SelectedItem));
        }

        public LogFilterViewModel(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
            _logFilterItems = new ObservableCollection<LogFilterItemViewModel>();
            for (int i = 0; i < 10; i++)
            {
                _logFilterItems.Add(new LogFilterItemViewModel()
                {
                    FilterKey = $"asdasd{i}",
                    IsMatchCase = true,
                    IsMatchWholeWord = true,
                    Hits = i
                });
            }
        }
    }
}