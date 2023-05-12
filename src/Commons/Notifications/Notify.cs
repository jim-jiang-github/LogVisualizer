using Avalonia.Controls.Notifications;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Notifications
{
    public static class Notify
    {
        public static IManagedNotificationManager? NotificationManager { get; set; }

        public static void NotifyError(string title, string content)
        {
            NotificationManager?.Show(new Notification(title, content, NotificationType.Error));
        }
    }
}
