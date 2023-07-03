using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using LogVisualizer.Commons;
using Serilog.Configuration;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogVisualizer
{
    public static class LogConfiguration
    {
        public class CallerEnricher : ILogEventEnricher
        {
            private readonly bool _includeFileInfo;
            private readonly int _maxDepth = 1;
            private readonly Predicate<MethodBase> _defaultFilter = method => method?.DeclaringType?.Assembly == typeof(Log).Assembly;

            private const int SkipFramesCount = 2;
            private const int MaxFrameCount = 128;

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Thread", new ScalarValue($"{Thread.CurrentThread.ManagedThreadId}")));

                int foundFrames = 0;
                StringBuilder caller = new StringBuilder();

                var stackTrace = EnhancedStackTrace.Current();

                var framesToProcess = stackTrace.Take(MaxFrameCount).Skip(SkipFramesCount);

                foreach (var frame in framesToProcess)
                {
                    if (!frame.HasMethod())
                    {
                        break;
                    }

                    var method = frame.MethodInfo;

                    if (_defaultFilter(method.MethodBase ?? method.SubMethodBase))
                    {
                        continue;
                    }

                    foreach (var param in method.Parameters)
                    {
                        param.Name = "";
                    }
                    foreach (var param in method.SubMethodParameters)
                    {
                        param.Name = "";
                    }

                    method.Append(caller, true);

                    if ((frame.GetFileName() is string callerFileName))
                    {
                        caller.Append($"{(_includeFileInfo ? $"{callerFileName} " : string.Empty)} line:{frame.GetFileLineNumber()}");
                    }

                    foundFrames++;

                    if (foundFrames >= _maxDepth)
                    {
                        break;
                    }
                }
                if (foundFrames > 0)
                {
                    logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue($"{caller}")));
                    return;
                }
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue("<unknown method>")));
                return;
            }
        }
        private class LogEventSink : ILogEventSink
        {
            private readonly INotify? _notify;

            public LogEventSink(INotify? notify) 
            {
                _notify = notify;
            }

            public void Emit(LogEvent logEvent)
            {
                if (logEvent.Level >= Serilog.Events.LogEventLevel.Error)
                {
                    _notify?.NotifyError("Error", logEvent.MessageTemplate.Text);
                }
            }
        }
        public static void Init(INotify? notify)
        {
            var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] [{Thread}] [{Caller}] {Message}" + Environment.NewLine;
            Log.Logger = new LoggerConfiguration()
                .Enrich.With<CallerEnricher>()
#if DEBUG
                .MinimumLevel.Debug()
                .WriteTo.Console()
#else
           .MinimumLevel.Information()
#endif
                .WriteTo.File($"{Global.AppDataDirectory}/logs/log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                outputTemplate: outputTemplate)
                .WriteTo.Sink(new LogEventSink(notify))
                .CreateLogger();
            Log.Information("Serilog is inited");
        }
        private static int GetCurrentLineNumber([CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }
    }
}
