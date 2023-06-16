using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class LogFilterItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _enabled;
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _hexColor = "#00000000";
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
