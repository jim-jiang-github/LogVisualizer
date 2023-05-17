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
                .AddSingleton<UpgradeService>()
                .AddSingleton<UpgraderViewModel>()
                .AddScoped<SideBarService>()
                .AddScoped<MenuBarService>()
                .AddScoped<MenuBarViewModel>()
                .AddScoped<MainWindowViewModel>()
                .AddScoped<SideBarViewModel>()
                .BuildServiceProvider();
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
