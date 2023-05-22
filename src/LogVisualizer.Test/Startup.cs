using LogVisualizer.Services;
using LogVisualizer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UpgradeService>();
            services.AddSingleton<UpgraderViewModel>();
            services.AddSingleton<GitService>();
            services.AddScoped<MenuBarViewModel>();
            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<SideBarViewModel>();
            services.AddScoped<BottomBarViewModel>();
            services.AddSingleton<SchemaConfigViewModel>();
        }
    }
}
