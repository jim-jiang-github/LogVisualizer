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
            for (int i = 0; i < 30; i++)
            {
                _logFilterItems.Add(new LogFilterItemViewModel()
                {
                    FilterKey = $"asdasd{i}",
                    IsMatchCase = i % 2 == 0,
                    HexColor = GetRandomHexColor(),
                    IsMatchWholeWord = i % 3 == 0,
                    IsUseRegularExpression = i % 5 == 0,
                    Hits = i
                });
            }
        }
        Random random = new Random();
        private string GetRandomHexColor()
        {
            Color randomColor = Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
            return $"#{ToUInt32(randomColor):x8}";
        }

        private uint ToUInt32(Color color)
        {
            return ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | (uint)color.B;
        }
        [RelayCommand]
        private async void DeleteLogFilterItem(LogFilterItemViewModel logFilterItemViewModel)
        {
            var content = I18NKeys.Common_ConfirmDelete.GetLocalizationString(logFilterItemViewModel.FilterKey);
            if (await Notify.ShowComfirmMessageBox(content))
            {
                LogFilterItems.Remove(logFilterItemViewModel);
            }
        }
    }
}