using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Commons.Attributes;
using Commons.Notifications;
using LogVisualizer.Platforms.Windows;
using Serilog;
using System;
using System.Runtime.InteropServices;

namespace LogVisualizer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            Notify.NotificationManager = new Avalonia.Controls.Notifications.WindowNotificationManager(this)
            {
                MaxItems = 6
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Notify.NotifyError("", "");
        }
    }
}