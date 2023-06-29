using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using LogVisualizer.I18N;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public static class Notify
    {
        public class MessageBoxButton
        {
            public string ButtonText { get; set; }
            public bool CloseOnClick { get; set; }

            public MessageBoxButton(string buttonText, bool closeOnClick = false)
            {
                ButtonText = buttonText;
                CloseOnClick = closeOnClick;
            }
        }

        private static Window _host;
        private static IManagedNotificationManager? NotificationManager { get; set; }

        public static void Init(Window host)
        {
            _host = host;
            NotificationManager = new WindowNotificationManager(host)
            {
                MaxItems = 6,
                Position = NotificationPosition.BottomRight
            };
        }

        public static void NotifyError(string title, string content)
        {
            Dispatcher.UIThread.Invoke(() =>
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

        public static Task<string?> ShowMessageBox(string? content, params MessageBoxButton[] buttons)
        {
            return ShowMessageBox("", content, buttons);
        }

        public async static Task<bool> ShowComfirmMessageBox(string? content)
        {
            var cancel = I18NKeys.Common_Cancel.GetLocalizationRawValue();
            var confirm = I18NKeys.Common_Confirm.GetLocalizationRawValue();
            var result = await ShowMessageBox("", content, new[]
            {
                new MessageBoxButton(cancel,true),
                new MessageBoxButton(confirm,true)
            });
            if (result == confirm)
            {
                return true;
            }
            return false;
        }

        public static Task<string?> ShowMessageBox(string? title, string? content, params MessageBoxButton[] buttons)
        {
            return Dispatcher.UIThread.InvokeAsync(() =>
            {
                var messageBoxMarkdownWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams()
                {
                    Icon = MessageBox.Avalonia.Enums.Icon.None,
                    CanResize = false,
                    MaxHeight = 400,
                    ContentHeader = title,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ContentMessage = content,
                    Markdown = true,
                    ButtonDefinitions = buttons.Select(x => new ButtonDefinition { Name = x.ButtonText, IsDefault = true, IsCancel = true })
                });
                return messageBoxMarkdownWindow.ShowDialog(_host);
            });
        }
    }
}
