using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using LogVisualizer.Commons.Notifications;
using LogVisualizer.ViewModels;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

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
        private bool flag = false;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (flag)
            {
                I18N.I18NManager.CurrentCulture = CultureInfo.GetCultureInfo("zh-CN");
            }
            else
            {
                I18N.I18NManager.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            }
            flag = !flag;
            Notify.NotifyCustom(DependencyInjectionProvider.GetService<UpgraderViewModel>());
        }
    }
}