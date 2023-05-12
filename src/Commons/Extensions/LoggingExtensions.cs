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
        public static T UseSerilog<T>(this T builder) where T : AppBuilderBase<T>, new()
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
