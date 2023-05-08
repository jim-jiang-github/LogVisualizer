using Avalonia.Controls;
using LogVisualizer.Platforms.Windows;
using System.Runtime.InteropServices;

namespace LogVisualizer.Views
{
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