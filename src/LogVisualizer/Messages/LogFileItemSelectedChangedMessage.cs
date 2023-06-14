using CommunityToolkit.Mvvm.Messaging.Messages;
using LogVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Messages
{
    public class LogFileItemSelectedChangedMessage : ValueChangedMessage<LogFileItem?>
    {
        public LogFileItemSelectedChangedMessage(LogFileItem? value) : base(value)
        {

        }
    }
}
