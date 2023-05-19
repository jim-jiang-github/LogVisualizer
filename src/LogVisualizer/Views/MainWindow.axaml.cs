using Avalonia.Controls;
using Avalonia.Input;
using LogVisualizer.Commons.Notifications;
using LogVisualizer.ViewModels;

namespace LogVisualizer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            Notify.NotificationManager = new Avalonia.Controls.Notifications.WindowNotificationManager(this)
            {
                MaxItems = 6,
                Position = Avalonia.Controls.Notifications.NotificationPosition.BottomRight
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Notify.NotifyCustom(DependencyInjectionProvider.GetService<UpgraderViewModel>());
        }
    }
}