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
                .AddScoped<MenuBarViewModel>()
                .AddScoped<LogSelectorViewModel>()
                .AddScoped<MainWindowViewModel>()
                .AddScoped<LogViewerViewModel>()
                .AddScoped<TitleBarViewModel>()
                .AddScoped<SideBarViewModel>()
                .AddScoped<BottomBarViewModel>()
                .AddSingleton<ScenarioConfigViewModel>()
                .AddSingleton<SplashWindowViewModel>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
            {
                serviceCollection.AddSingleton<UpgradeProgress, UpgradeProgressWindows>();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
            {
                serviceCollection.AddSingleton<UpgradeProgress, UpgradeProgressOSX>();
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
