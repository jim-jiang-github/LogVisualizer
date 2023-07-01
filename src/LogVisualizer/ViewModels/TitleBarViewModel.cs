using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.I18N;
using LogVisualizer.Models;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class TitleBarViewModel : ViewModelBase
    {
        private readonly FinderService _finderService;

        [ObservableProperty]
        private bool _enablePseudo;

        public TitleBarViewModel(FinderService finderService)
        {
            _finderService = finderService;
        }
        partial void OnEnablePseudoChanged(bool oldValue, bool newValue)
        {
            I18NManager.EnablePseudo = newValue;
        }

        [RelayCommand]
        private void AccessGithub()
        {
            _finderService.AccessGithub();
        }

        [RelayCommand]
        private void Share()
        {
        }
    }
}
