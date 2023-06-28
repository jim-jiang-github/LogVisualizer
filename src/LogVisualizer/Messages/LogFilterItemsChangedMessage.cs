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
    public class LogFilterItemsChangedMessage : ValueChangedMessage<IEnumerable<LogFilterItemViewModel>>
    {
        public LogFilterItemsChangedMessage(IEnumerable<LogFilterItemViewModel> value) : base(value)
        {

        }
    }
}
