using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using System.Collections;
using LogVisualizer.Services;
using LogVisualizer.Views;
using System.Collections.ObjectModel;
using LogVisualizer.Models;

namespace LogVisualizer.ViewModels
{
    public class LogSelectorViewModel : ViewModelBase
    {
        public ObservableCollection<LogItem> LogItems { get; }

        public LogSelectorViewModel()
        {
            LogItems = new ObservableCollection<LogItem>();
            for (int i = 0; i < 20; i++)
            {
                LogItems.Add(new LogItem()
                {
                    Name = $"Log{i}"
                });
            }
        }
    }
}