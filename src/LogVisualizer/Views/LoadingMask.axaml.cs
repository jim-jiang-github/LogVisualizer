using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.ViewModels;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LogVisualizer.Views
{
    public class LoadingMaskBindingExtension : MarkupExtension
    {
        public class LoadingMaskSource : INotifyPropertyChanged
        {
            private static LoadingMaskSource _instance;
            public static LoadingMaskSource Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new LoadingMaskSource();
                    }
                    return _instance;
                }
            }

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

            private LoadingMaskSource()
            {

            }
        }
        public LoadingMaskBindingExtension()
        {

        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new ReflectionBindingExtension(nameof(LoadingMaskSource.ShowLoading))
            {
                Source = LoadingMaskSource.Instance,
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
    public partial class LoadingMask : UserControl
    {
        public LoadingMask()
        {
            InitializeComponent();
        }
    }
}
