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

namespace LogVisualizer.ViewModels
{
    public class LogDisplayViewModel : ViewModelBase
    {
        public IList? Items { get; }

        public LogDisplayViewModel()
        {
            //Items = scenario.LogSource?.GetRows();
        }
    }

}