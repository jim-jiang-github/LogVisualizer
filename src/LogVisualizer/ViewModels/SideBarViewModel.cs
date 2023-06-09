using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class SideBarViewModel : ViewModelBase
    {
        public LogSelectorViewModel LogSelector { get; }
        public SideBarViewModel(LogSelectorViewModel logSelectorViewModel)
        {
            LogSelector = logSelectorViewModel;
        }

        [RelayCommand]
        public void A() { }
    }
}
