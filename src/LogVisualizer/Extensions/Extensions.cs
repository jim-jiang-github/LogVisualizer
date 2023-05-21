using Avalonia;
using LogVisualizer.Commons.Notifications;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.Views;

namespace LogVisualizer.Extensions
{
    public static class TaskExtensions
    {
        public async static Task<T> WithLoadingMask<T>(this Task<T> task)
        {
            LoadingMaskBindingExtension.LoadingMaskSource.Instance.ShowLoading = true;
            await task;
            LoadingMaskBindingExtension.LoadingMaskSource.Instance.ShowLoading = false;
            return task.Result;
        }

        public async static Task WithLoadingMask(this Task task)
        {
            LoadingMaskBindingExtension.LoadingMaskSource.Instance.ShowLoading = true;
            await task;
            LoadingMaskBindingExtension.LoadingMaskSource.Instance.ShowLoading = false;
        }
    }
}
