using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LogVisualizer.Platforms.Windows
{
    public partial class WindowsTitleBar : UserControl
    {
        public WindowsTitleBar()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
            {
                var rootWindow = VisualRoot as Window;
                rootWindow.ExtendClientAreaToDecorationsHint = true;
                rootWindow.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
                rootWindow.ExtendClientAreaTitleBarHeightHint = -1;
                IsVisible = true;
            }
        }
    }
}
