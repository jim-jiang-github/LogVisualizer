using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public partial class LoadingBindingSource : ObservableObject
    {
        public static LoadingBindingSource BindingSource { get; } = new LoadingBindingSource();

        [ObservableProperty]
        private bool _showLoading = false;
        public bool IsIndeterminate => Value == 0;
        [ObservableProperty]
        private double _value = 0;
        [ObservableProperty]
        private string _message = string.Empty;

        partial void OnValueChanged(double oldValue, double newValue)
        {
            OnPropertyChanged(nameof(IsIndeterminate));
        }

        internal LoadingBindingSource()
        {

        }
    }
    public static class Loading
    {
        public static double? ProgressPercentage { get; set; }

        public static void ShowLoading()
        {
            Reset();
            LoadingBindingSource.BindingSource.ShowLoading = true;
        }

        public static void SetProgress(double progress)
        {
            LoadingBindingSource.BindingSource.Value = progress * 100;
        }

        public static void SetMessage(string message)
        {
            LoadingBindingSource.BindingSource.Message = message;
        }

        public static void HideLoading()
        {
            LoadingBindingSource.BindingSource.ShowLoading = false;
            Reset();
        }

        private static void Reset()
        {
            LoadingBindingSource.BindingSource.Value = 0;
            LoadingBindingSource.BindingSource.Message = string.Empty;
        }
    }
}
