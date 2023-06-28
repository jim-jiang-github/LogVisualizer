using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Models;
using LogVisualizer.Scenarios.Contents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class LogRowDetailViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<LogRowDetailProperty> _logRowDetailProperties;

        public LogRowDetailViewModel()
        {
            LogRowDetailProperties = new ObservableCollection<LogRowDetailProperty>();
        }

        public void SetLogRow(string[] header, LogRow logRow)
        {
            if (header.Length != logRow.Cells.Length)
            {
                Log.Error($"Header is [{string.Join(",", header)}] but LogRow is [{string.Join(",", logRow.Cells)}]");
                return;
            }
            LogRowDetailProperties.Clear();
            for (int i = 0; i < header.Length; i++)
            {
                LogRowDetailProperties.Add(new LogRowDetailProperty()
                {
                    PropertyName = header[i],
                    PropertyValue = logRow.Cells[i]?.ToString()
                });
            }
        }
    }
}
