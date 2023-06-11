using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.ViewModels;
using System.Threading.Tasks;
using System.ComponentModel;
using LogVisualizer.Commons;

namespace LogVisualizer.Views
{
    public partial class LoadingMask : UserControl
    {
        public LoadingMask()
        {
            InitializeComponent();
            DataContext = LoadingBindingSource.BindingSource;
        }
    }
}
