using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public interface INotify
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
        void NotifyError(string title, string content);
        void NotifyCustom(object viewModel);
        Task<string?> ShowMessageBox(string? content, params MessageBoxButton[] buttons);
        Task<bool> ShowComfirmMessageBox(string? content);
        Task<string?> ShowMessageBox(string? title, string? content, params MessageBoxButton[] buttons);
        Task<string?> ShowSubWindow(string? title, object viewModel, params MessageBoxButton[] buttons);
        Task ShowSubWindow(string? title, object viewModel);
    }
}
