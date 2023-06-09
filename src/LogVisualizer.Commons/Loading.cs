using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public static class Loading
    {
        public class LoadingBindingSource : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            private bool _showLoading = false;

            public bool ShowLoading
            {
                get => _showLoading;
                set
                {
                    _showLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowLoading)));
                }
            }

            internal LoadingBindingSource()
            {

            }
        }
        public static LoadingBindingSource BindingSource { get; } = new LoadingBindingSource();

        public static void ShowLoading()
        {
            BindingSource.ShowLoading = true;
        }

        public static void HideLoading()
        {
            BindingSource.ShowLoading = false;
        }
    }
}
