using GithubReleaseUpgrader;
using LogVisualizer.Platforms.Windows;
using LogVisualizer.Services;
using LogVisualizer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer
{
    internal static class DependencyInjectionProvider
    {
        private static IServiceProvider? _serviceProvider;

        public static void Init()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<ScenarioService>()
                .AddSingleton<UpgradeService>()
                .AddSingleton<GitService>()
                .AddSingleton<MenuBarViewModel>()
                .AddSingleton<LogSelectorViewModel>()
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<LogViewerViewModel>()
                .AddSingleton<LogFilterViewModel>()
                .AddSingleton<TitleBarViewModel>()
                .AddSingleton<SideBarViewModel>()
                .AddScoped<LogFilterItemEditorViewModel>()
                .AddSingleton<BottomBarViewModel>()
                .AddSingleton<ScenarioConfigViewModel>()
                .AddSingleton<SplashWindowViewModel>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
            {
                serviceCollection.AddSingleton<UpgradeHandler, UpgradeHandlerWindows>();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
            {
                serviceCollection.AddSingleton<UpgradeHandler, UpgradeHandlerOSX>();
            }
            _serviceProvider = serviceCollection.BuildServiceProvider();
            Log.Information("DependencyInjectionProvider inited!");
        }

        public static T? GetService<T>()
        {
            if (_serviceProvider == null)
            {
                return default;
            }
            return _serviceProvider.GetService<T>();
        }
    }
}
