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

namespace LogVisualizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MenuBarViewModel MenuBar { get; }
        public SideBarViewModel SideBar { get; }
        public BottomBarViewModel BottomBar { get; }
        public LogDisplayViewModel CurrentLog { get; }

        public MainWindowViewModel(
            MenuBarViewModel menuBarViewModel,
            SideBarViewModel sideBarViewModel,
            BottomBarViewModel bottomBarViewModel,
            LogDisplayViewModel logDisplayViewModel)
        {
            MenuBar = menuBarViewModel;
            SideBar = sideBarViewModel;
            BottomBar = bottomBarViewModel;
            CurrentLog = logDisplayViewModel;
        }
    }
}