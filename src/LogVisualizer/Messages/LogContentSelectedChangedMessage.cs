﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using LogVisualizer.Models;
using LogVisualizer.Scenarios.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Messages
{
    public class LogContentSelectedChangedMessage : ValueChangedMessage<ILogContent?>
    {
        public LogContentSelectedChangedMessage(ILogContent? value) : base(value)
        {

        }
    }
}
