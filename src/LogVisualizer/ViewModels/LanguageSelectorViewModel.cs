using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.I18N;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class LanguageSelectorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private CultureInfo _currentCulture;

        public IEnumerable<CultureInfo> SupportCultures { get; } = I18NManager.SupportCultureList;

        public LanguageSelectorViewModel()
        {
            CurrentCulture = I18NManager.CurrentCulture;
        }

        partial void OnCurrentCultureChanged(CultureInfo value)
        {
            I18NManager.CurrentCulture = value;
        }
    }
}
