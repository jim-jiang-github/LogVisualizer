using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using Avalonia;
using System.Collections;
using LogVisualizer.Services;
using LogVisualizer.Views;
using Avalonia.Controls.Chrome;
using System.Diagnostics;
using Avalonia.OpenGL;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LogVisualizer.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _title = $"LogVisualizer({Assembly.GetEntryAssembly()?.GetName()?.Version})";

        public TitleBarViewModel TitleBar { get; }
        public MenuBarViewModel MenuBar { get; }
        public SideBarViewModel SideBar { get; }
        public BottomBarViewModel BottomBar { get; }
        public LogViewerViewModel LogViewer { get; }
        public LogFilterViewModel LogFilter { get; }

        public MainWindowViewModel(
            TitleBarViewModel titleBarViewModel,
            MenuBarViewModel menuBarViewModel,
            SideBarViewModel sideBarViewModel,
            BottomBarViewModel bottomBarViewModel,
            LogViewerViewModel logViewerViewModel,
            LogFilterViewModel logFilterViewModel)
        {
            TitleBar = titleBarViewModel;
            MenuBar = menuBarViewModel;
            SideBar = sideBarViewModel;
            BottomBar = bottomBarViewModel;
            LogViewer = logViewerViewModel;
            LogFilter = logFilterViewModel;
        }
    }
}