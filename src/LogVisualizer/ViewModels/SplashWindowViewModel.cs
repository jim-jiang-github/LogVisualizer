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

namespace LogVisualizer.ViewModels
{
    public partial class SplashWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _message;

        public SplashWindowViewModel()
        {
        }

        public async Task Execute()
        {
            Message = "1111111111111111111";
            await Task.Delay(1000);
            Message = "2222222222222222222";
            await Task.Delay(1000);
        }
    }
}