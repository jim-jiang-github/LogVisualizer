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
    public partial class OSXTitleBar : UserControl
    {
        public OSXTitleBar()
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
            {
                var rootWindow = VisualRoot as Window;
                if (rootWindow == null)
                {
                    return;
                }
                rootWindow.ExtendClientAreaToDecorationsHint = true;
                rootWindow.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
                rootWindow.ExtendClientAreaTitleBarHeightHint = -1;
                IsVisible = true;
            }
        }
    }
}
