using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using LogVisualizer.Commons;
using LogVisualizer.Commons.Attributes;
using LogVisualizer.ViewModels;
using LogVisualizer.Views;
using System.Threading.Tasks;

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
        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var splashWindowViewModel = DependencyInjectionProvider.GetService<SplashWindowViewModel>();
                SplashWindow splashWindow = new()
                {
                    DataContext = splashWindowViewModel
                };
                if (splashWindowViewModel != null)
                {
                    desktop.MainWindow = splashWindow;
                    splashWindow.Show();
                    await splashWindowViewModel.Execute();
                }
                desktop.MainWindow = new MainWindow
                {
                    DataContext = DependencyInjectionProvider.GetService<MainWindowViewModel>()
                };
                desktop.MainWindow.Show();
                splashWindow?.Close();
                GlobalStorageProvider.StorageProvider = desktop.MainWindow.StorageProvider;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}