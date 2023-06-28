using CommunityToolkit.Mvvm.Messaging.Messages;
using LogVisualizer.Models;
using LogVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Messages
{
    public class LogFilterItemDetailSelectedChangedMessage : ValueChangedMessage<LogFilterItemViewModel>
    {
        private readonly TaskCompletionSource<bool> _completionSource = new TaskCompletionSource<bool>();

        public Task<bool> Response => _completionSource.Task;

        public bool AddNew { get; }

        public void SetResponse(bool response)
        {
            _completionSource.SetResult(response);
        }

        public LogFilterItemDetailSelectedChangedMessage(LogFilterItemViewModel value, bool addNew = false) : base(value)
        {
            AddNew = addNew;
        }
    }
}
