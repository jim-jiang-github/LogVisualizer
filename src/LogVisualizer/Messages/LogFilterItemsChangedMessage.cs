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
    public class LogFilterItemsChangedMessage : ValueChangedMessage<IEnumerable<LogFilterItem>>
    {
        public LogFilterItemsChangedMessage(IEnumerable<LogFilterItem> value) : base(value)
        {

        }
    }
}
