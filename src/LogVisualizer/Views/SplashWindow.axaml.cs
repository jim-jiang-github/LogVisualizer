using Avalonia.Controls;

namespace LogVisualizer.Views
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
#if RELEASE
            this.Topmost = true;
#endif
        }
    }
}
