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
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using LogVisualizer.Commons;

namespace LogVisualizer.ViewModels
{
    public partial class SplashWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _message;

        private readonly ScenarioService _scenarioService;

        public SplashWindowViewModel(ScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        public async Task Execute()
        {
            await Task.Delay(1000);
        }
    }
}