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
            _logRowDetailProperties = new ObservableCollection<LogRowDetailProperty>();
        }

        public void SetLogRow(string[] columnNames, LogRow logRow)
        {
            if (columnNames.Length != logRow.Cells.Length)
            {
                Log.Error($"ColumnNames is [{string.Join(",", columnNames)}] but LogRow is [{string.Join(",", logRow.Cells)}]");
                return;
            }
            LogRowDetailProperties.Clear();
            for (int i = 0; i < columnNames.Length; i++)
            {
                LogRowDetailProperties.Add(new LogRowDetailProperty()
                {
                    PropertyName = columnNames[i],
                    PropertyValue = logRow.Cells[i]?.ToString()
                });
            }
        }
    }
}
