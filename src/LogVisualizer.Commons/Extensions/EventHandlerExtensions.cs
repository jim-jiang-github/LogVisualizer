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

namespace System
{
    public delegate void EventHandler<TEventArgs1, TEventArgs2>(object? sender, TEventArgs1 e1, TEventArgs2 e2);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9, TEventArgs10>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9, TEventArgs3 e10);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9, TEventArgs10, TEventArgs11>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9, TEventArgs3 e10, TEventArgs3 e11);
    public delegate void EventHandler<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9, TEventArgs10, TEventArgs11, TEventArgs12>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9, TEventArgs3 e10, TEventArgs3 e11, TEventArgs3 e12);
   
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2>(object? sender, TEventArgs1 e1, TEventArgs2 e2);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9, TEventArgs10>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9, TEventArgs3 e10);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9, TEventArgs10, TEventArgs11>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9, TEventArgs3 e10, TEventArgs3 e11);
    public delegate Task EventHandlerAsync<TEventArgs1, TEventArgs2, TEventArgs3, TEventArgs4, TEventArgs5, TEventArgs6, TEventArgs7, TEventArgs8, TEventArgs9, TEventArgs10, TEventArgs11, TEventArgs12>(object? sender, TEventArgs1 e1, TEventArgs2 e2, TEventArgs3 e3, TEventArgs3 e4, TEventArgs3 e5, TEventArgs3 e6, TEventArgs3 e7, TEventArgs3 e8, TEventArgs3 e9, TEventArgs3 e10, TEventArgs3 e11, TEventArgs3 e12);
}