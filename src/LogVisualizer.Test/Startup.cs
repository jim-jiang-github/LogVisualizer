using LogVisualizer.Services;
using LogVisualizer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace LogVisualizer.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ScenarioService>()
                .AddSingleton<UpgradeService>()
                .AddSingleton<GitService>()
                .AddScoped<MenuBarViewModel>()
                .AddScoped<MainWindowViewModel>()
                .AddScoped<LogViewerViewModel>()
                .AddScoped<SideBarViewModel>()
                .AddScoped<BottomBarViewModel>()
                .AddSingleton<ScenarioConfigViewModel>()
                .AddSingleton<SplashWindowViewModel>();
        }

        public Startup() 
        {
        
        }
    }
}
