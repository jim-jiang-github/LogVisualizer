﻿using Avalonia;
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
    public static class LoggingExtensions
    {
        private class LogEventSink : ILogEventSink
        {
            public void Emit(LogEvent logEvent)
            {
                if (logEvent.Level >= Serilog.Events.LogEventLevel.Error)
                {
                    Notify.NotifyError("Error", logEvent.MessageTemplate.Text);
                }
            }
        }
        public static AppBuilder UseSerilog(this AppBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                       .MinimumLevel.Debug()
                       .WriteTo.Console()
#else
           .MinimumLevel.Information()
#endif
                       .WriteTo.File("logs/log.txt",
                       rollingInterval: RollingInterval.Day,
                       rollOnFileSizeLimit: true)
                       .WriteTo.Sink(new LogEventSink())
                       .CreateLogger();
            return builder;
        }
    }
}