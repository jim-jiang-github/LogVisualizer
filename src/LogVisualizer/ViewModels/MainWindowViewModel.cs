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

namespace LogVisualizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TitleBarViewModel TitleBar { get; }
        public MenuBarViewModel MenuBar { get; }
        public SideBarViewModel SideBar { get; }
        public BottomBarViewModel BottomBar { get; }
        public LogViewerViewModel LogViewer { get; }

        public MainWindowViewModel(
            TitleBarViewModel titleBarViewModel,
            MenuBarViewModel menuBarViewModel,
            SideBarViewModel sideBarViewModel,
            BottomBarViewModel bottomBarViewModel,
            LogViewerViewModel logDisplayViewModel)
        {
            TitleBar = titleBarViewModel;
            MenuBar = menuBarViewModel;
            SideBar = sideBarViewModel;
            BottomBar = bottomBarViewModel;
            LogViewer = logDisplayViewModel;
        }
    }
}