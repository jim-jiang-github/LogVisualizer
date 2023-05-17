using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Commons.Notifications;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    public static class NotifyExtensions
    {
        public static AppBuilder UseNotify(this AppBuilder builder)
        {
          
            return builder;
        }
    }
}
