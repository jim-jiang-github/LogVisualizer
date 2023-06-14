using CommunityToolkit.Mvvm.Messaging.Messages;
using LogVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Messages
{
    public class LogFileItemsChangedMessage : ValueChangedMessage<IEnumerable<LogFileItem>>
    {
        public LogFileItemsChangedMessage(IEnumerable<LogFileItem> value) : base(value)
        {

        }
    }
}
