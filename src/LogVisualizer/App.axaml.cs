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
using Metalama.Framework.Services;

namespace LogVisualizer
{
    public partial class App : Application
    {
        public IServiceProvider? _serviceProvider;

        [Log]
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            _serviceProvider = new ServiceCollection()
                .AddScoped<SideBarService>()
                .AddScoped<MenuBarService>()
                .AddScoped<MainWindowViewModel>()
                .AddScoped<SideBarViewModel>()
                .BuildServiceProvider();
        }

        [Log]
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && _serviceProvider != null)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = ActivatorUtilities.CreateInstance<MainWindowViewModel>(_serviceProvider)
                };
                Notify.NotificationManager = new Avalonia.Controls.Notifications.WindowNotificationManager(desktop.MainWindow)
                {
                    Position = Avalonia.Controls.Notifications.NotificationPosition.BottomRight,
                    MaxItems = 6
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}