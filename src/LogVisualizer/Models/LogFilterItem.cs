using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Models
{
    public partial class LogFilterItem : ModelBase
    {
        [ObservableProperty]
        private bool _enabled = true;
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _hexColor = "#FFFFFFFF";
        [ObservableProperty]
        private int _hits;
        [ObservableProperty]
        private string _filterKey = string.Empty;
        [ObservableProperty]
        private bool _isMatchCase = false;
        [ObservableProperty]
        private bool _isMatchWholeWord = false;
        [ObservableProperty]
        private bool _isUseRegularExpression = false;
    }
}
