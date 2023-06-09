using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.ViewModels;
using System.Threading.Tasks;
using System.ComponentModel;
using static LogVisualizer.Commons.Loading;
using LogVisualizer.Commons;

namespace LogVisualizer.Views
{
    public class LoadingMaskBindingExtension : MarkupExtension
    {
        public LoadingMaskBindingExtension()
        {

        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new ReflectionBindingExtension(nameof(LoadingBindingSource.ShowLoading))
            {
                Source = Loading.BindingSource,
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
