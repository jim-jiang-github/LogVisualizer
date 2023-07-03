using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Threading;
using LogVisualizer.CustomControls;
using LogVisualizer.I18N;
using LogVisualizer.Messages;
using LogVisualizer.ViewModels;
using LogVisualizer.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    internal class NotifyImpl : INotify
    {
        private static IManagedNotificationManager? NotificationManager { get; set; }

        private Window? _host;

        public Window Host
        {
            get
            {
                if (_host == null)
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                        {
                            _host = desktop.MainWindow;
                            NotificationManager = new WindowNotificationManager(_host)
                            {
                                MaxItems = 6,
                                Position = NotificationPosition.BottomRight
                            };
                        }
                    });
                }
                if (_host == null)
                {
                    throw new NotImplementedException("Can not get host Window.");
                }
                return _host;
            }
        }

        public NotifyImpl()
        {
        }

        public void NotifyCustom(object viewModel)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                NotificationManager?.Show(viewModel);
            });
        }

        public void NotifyError(string title, string content)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                NotificationManager?.Show(new Notification(title, content, NotificationType.Error));
            });
        }

        public Task<string?> ShowMessageBox(string? content, params MessageBoxButton[] buttons)
        {
            return ShowMessageBox("", content, buttons);
        }

        public async Task<bool> ShowComfirmMessageBox(string? content)
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

        public Task<string?> ShowMessageBox(string? title, string? content, params MessageBoxButton[] buttons)
        {
            if (Host == null)
            {
                return Task.FromResult<string?>(null);
            }
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
                return messageBoxMarkdownWindow.ShowDialog(Host);
            });
        }

        public async Task<string?> ShowSubWindow(string? title, object viewModel, params MessageBoxButton[] buttons)
        {
            return await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                SubDialogWindow subDialogWindow = new SubDialogWindow();
                var template = subDialogWindow.DataTemplates.OfType<DataTemplate>().FirstOrDefault(x => x.DataType == viewModel.GetType());
                if (template == null || template.Build(template.Content) is not Control control)
                {
                    return null;
                }
                control.DataContext = viewModel;
                subDialogWindow.Content = control;
                subDialogWindow.Title = title;
                subDialogWindow.Buttons = new ObservableCollection<MessageBoxButton>(buttons);
                var clickedButtonText = await subDialogWindow.ShowDialogAsync(Host);
                return clickedButtonText;
            });
        }

        public Task ShowSubWindow(string? title, object viewModel)
        {
            return Dispatcher.UIThread.InvokeAsync(async () =>
            {
                SubDialogWindow subDialogWindow = new SubDialogWindow();
                var template = subDialogWindow.DataTemplates.OfType<DataTemplate>().FirstOrDefault(x => x.DataType == viewModel.GetType());
                if (template == null || template.Build(template.Content) is not Control control)
                {
                    return;
                }
                control.DataContext = viewModel;
                subDialogWindow.Content = control;
                subDialogWindow.Title = title;
                await subDialogWindow.ShowDialogAsync(Host);
            });
        }
    }
}
