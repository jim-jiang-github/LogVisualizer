using Avalonia;
using System;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using Projektanker.Icons.Avalonia.MaterialDesign;
using LogVisualizer.Services;
using LogVisualizer.Commons.Notifications;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using LogVisualizer.Commons;

namespace LogVisualizer
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            LogConfiguration.Init();
            Log.Information("Program Main start!");
            Configuration.CreateInstance();
            DependencyInjectionProvider.Init();
            var upgradeService = DependencyInjectionProvider.GetService<UpgradeService>();
            var isNeedUpgrade = upgradeService?.CheckForUpgrade() ?? false;
            if (!isNeedUpgrade)
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }

            Log.Information("Program Main end!");
            upgradeService?.PerformUpgradeIfNeeded();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithIcons(container => container
            .Register<FontAwesomeIconProvider>()
            .Register<MaterialDesignIconProvider>()
            .Register<CustomIconProvider>());
    }
}