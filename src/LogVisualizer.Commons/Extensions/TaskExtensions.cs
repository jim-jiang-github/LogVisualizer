using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons.Extensions
{
    public static class TaskExtensions
    {
        public async static Task<T> WithLoadingMask<T>(this Task<T> task)
        {
            Loading.ShowLoading();
            await task;
            await Task.Delay(200);
            Loading.HideLoading();
            return task.Result;
        }

        public async static Task WithLoadingMask(this Task task)
        {
            Loading.ShowLoading();
            await task;
            await Task.Delay(200);
            Loading.HideLoading();
        }
    }
}
