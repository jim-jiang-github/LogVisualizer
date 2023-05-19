using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons.Notifications
{
    public static class Notify
    {
        public static IManagedNotificationManager? NotificationManager { get; set; }

        public static void NotifyError(string title, string content)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                NotificationManager?.Show(new Notification(title, content, NotificationType.Error));
            });
        }
        public static void NotifyCustom(object viewModel)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                NotificationManager?.Show(viewModel);
            });
        }
    }
}
