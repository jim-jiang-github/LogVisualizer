using LogVisualizer.Services;
using LogVisualizer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer
{
    internal static class DependencyInjectionProvider
    {
        private static IServiceProvider? _serviceProvider;

        public static void Init()
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<ScenarioService>()
                .AddSingleton<UpgradeService>()
                .AddSingleton<GitService>()
                .AddScoped<MenuBarViewModel>()
                .AddScoped<LogSelectorViewModel>()
                .AddScoped<MainWindowViewModel>()
                .AddScoped<LogDisplayViewModel>()
                .AddScoped<SideBarViewModel>()
                .AddScoped<BottomBarViewModel>()
                .AddSingleton<SchemaConfigViewModel>()
                .AddSingleton<SplashWindowViewModel>()
                .BuildServiceProvider();
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
