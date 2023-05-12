using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Commons.Attributes;
using Commons.Notifications;
using LogVisualizer.Platforms.Windows;
using Serilog;
using System;
using System.Runtime.InteropServices;

namespace LogVisualizer.Views
{
    public class ListBoxItemProperties : AvaloniaObject
    {
        public static readonly AttachedProperty<int> IndexProperty =
            AvaloniaProperty.RegisterAttached<ListBoxItemProperties, ListBoxItem, int>("Index");

        public static int GetIndex(ListBoxItem item)
        {
            return 1;
        }

        public static void SetIndex(ListBoxItem item, int value)
        {
            throw new NotImplementedException();
        }
        
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var windowsTitleBar = this.FindControl<WindowsTitleBar>("WindowsTitleBar");
            windowsTitleBar.IsVisible = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
            {
                ExtendClientAreaToDecorationsHint = true;
                ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
                ExtendClientAreaTitleBarHeightHint = -1;
                windowsTitleBar.IsVisible = true;
            }
        }
    }
}