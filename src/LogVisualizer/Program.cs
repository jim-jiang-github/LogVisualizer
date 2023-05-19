using Avalonia;
using System;
using Commons.Extensions;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using GithubReleaseUpgrader;
using LogVisualizer.Services;

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
            Configuration.CreateInstance();
            DependencyInjectionProvider.Init();
            var upgradeService = DependencyInjectionProvider.GetService<UpgradeService>();
            var isNeedUpgrade = upgradeService?.CheckForUpgrade() ?? false;
            if (!isNeedUpgrade)
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }

            upgradeService?.PerformUpgradeIfNeeded();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseSerilog()
            .WithIcons(container => container.Register<FontAwesomeIconProvider>());
    }
}