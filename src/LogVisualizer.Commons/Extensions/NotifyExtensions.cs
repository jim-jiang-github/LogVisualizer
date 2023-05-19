using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using LogVisualizer.Commons.Notifications;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons.Extensions
{
    public static class NotifyExtensions
    {
        public static AppBuilder UseNotify(this AppBuilder builder)
        {
          
            return builder;
        }
    }
}
