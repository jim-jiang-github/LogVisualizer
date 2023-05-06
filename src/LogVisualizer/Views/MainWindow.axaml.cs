using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
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

      

        private void Xx_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            //if (xx.SelectedItems.Contains(e.Row))
            //{
            //    e.Row.Background = Brushes.Yellow;
            //    e.Row.Foreground = Brushes.Blue;
            //}
        }
    }
}