using Avalonia.Controls;
using System.Runtime.InteropServices;

namespace LogVisualizer.CustomControls
{
    public partial class SubDialogWindow : Window
    {
        public SubDialogWindow()
        {
            InitializeComponent();
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
        }
    }
}
