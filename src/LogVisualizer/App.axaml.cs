using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LogVisualizer.Commons;
using LogVisualizer.Commons.Attributes;
using LogVisualizer.ViewModels;
using LogVisualizer.Views;

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
                GlobalStorageProvider.StorageProvider = desktop.MainWindow.StorageProvider;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}