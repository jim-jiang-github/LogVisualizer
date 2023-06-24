using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class LogFilterItemEditorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private LogFilterItemViewModel _logFilterItem;
        [ObservableProperty]
        private ObservableCollection<string> _keyWords = new ObservableCollection<string>();

        public LogFilterItemEditorViewModel()
        {
        }
    }
}
