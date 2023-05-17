using Avalonia.Controls;
using Commons;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class MenuBarViewModel : ViewModelBase
    {
        [RelayCommand]
        public void Open()
        {
        }

        [RelayCommand]
        public void Exit()
        {
        }

        [RelayCommand]
        public void ShowOnlyFiltered()
        {
        }

        [RelayCommand]
        public void AddNewFilter()
        {
        }

        [RelayCommand]
        public void About()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = Global.GITHUB_URL,
                UseShellExecute = true
            };
            process.Start();
        }
    }
}
