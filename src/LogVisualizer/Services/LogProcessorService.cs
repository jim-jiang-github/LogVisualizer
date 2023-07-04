using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.Messages;
using LogVisualizer.Models;
using LogVisualizer.Scenarios.Contents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class LogProcessorService
    {
        private readonly FilterService _filterService;
        private string[] _columnNames;
        private int _mainColumnIndex = 0;
        private IEnumerable<LogRow> _totalRows = Array.Empty<LogRow>();

        public IEnumerable<LogRow> DisplayRows
        {
            get
            {
                return _totalRows.Where(row =>
                {
                    var mainCell = row.Cells[_mainColumnIndex];
                    if (mainCell?.ToString() is not string mainCellStr)
                    {
                        return true;
                    }
                    if (!_filterService.LogFilterItems.Any(x => x.Enabled))
                    {
                        return true;
                    }
                    bool matched = _filterService.LogFilterItems
                    .Where(f => f.Enabled)
                    .Any(f => _filterService.Search(mainCellStr, f.FilterKey, f.IsMatchCase, f.IsMatchWholeWord, f.IsUseRegularExpression));
                    return matched;
                });
            }
        }

        public LogProcessorService(FilterService filterService)
        {
            _filterService = filterService;
            WeakReferenceMessenger.Default.Register<LogContentSelectedChangedMessage>(this, (r, m) =>
            {
                var logContent = m.Value;
                if (logContent == null)
                {
                    return;
                }
                _columnNames = logContent.ColumnNames;
                _totalRows = logContent.Rows;
                _mainColumnIndex = logContent.MainColumnIndex;
                NotifyDisplayRowsChanged();
            });
            WeakReferenceMessenger.Default.Register<LogFilterItemsChangedMessage>(this, (r, m) =>
            {
                NotifyDisplayRowsChanged();
            });
        }

        private void NotifyDisplayRowsChanged()
        {
            WeakReferenceMessenger.Default.Send(new LogDisplayRowsChangedMessage(DisplayRows)
            {
                ColumnNames = _columnNames,
                MainColumnIndex = _mainColumnIndex
            }); 
        }
    }
}
