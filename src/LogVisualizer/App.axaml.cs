using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Commons.Attributes;
using Commons.Notifications;
using LogVisualizer.ViewModels;
using LogVisualizer.Views;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using LogVisualizer.Services;

namespace LogVisualizer
{
    public partial class App : Application
    {
        [LogInfo]
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        [LogInfo]
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = DependencyInjectionProvider.GetService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}