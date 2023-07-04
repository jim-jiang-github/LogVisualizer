using CommunityToolkit.Mvvm.Messaging.Messages;
using LogVisualizer.Models;
using LogVisualizer.Scenarios.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Messages
{
    public class LogDisplayRowsChangedMessage : ValueChangedMessage<IEnumerable<LogRow>>
    {
        public string[] ColumnNames { get; set; }
        public int MainColumnIndex { get; set; } = 0;
        public LogDisplayRowsChangedMessage(IEnumerable<LogRow> value) : base(value)
        {

        }
    }
}
