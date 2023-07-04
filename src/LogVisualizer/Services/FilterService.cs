using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.Commons;
using LogVisualizer.I18N;
using LogVisualizer.Messages;
using LogVisualizer.Models;
using LogVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class FilterService
    {
        private readonly INotify _notify;
        private readonly DebounceDispatcher _debounceDispatcher;
        private readonly List<LogFilterItem> _logFilterItems = new();

        public IEnumerable<LogFilterItem> LogFilterItems => _logFilterItems;

        public FilterService(INotify notify)
        {
            _notify = notify;
            _debounceDispatcher = new DebounceDispatcher();
        }

        public async Task<LogFilterItem?> CreateFilterItem(string filterKey)
        {
            LogFilterItem logFilterItem = new()
            {
                FilterKey = filterKey,
            };
            var confirmButtonText = I18NKeys.Common_Confirm.GetLocalizationRawValue();
            var clickedButtonTest = await _notify.ShowSubWindow(
                I18NKeys.Filter_Dialog_Create.GetLocalizationRawValue(),
                new LogFilterItemEditorViewModel()
                {
                    LogFilterItem = logFilterItem
                },
                new[]
                {
                    new MessageBoxButton(confirmButtonText, true)
                });
            if (clickedButtonTest != confirmButtonText)
            {
                return null;
            }
            AddFilterItem(logFilterItem);
            return logFilterItem;
        }

        public void AddFilterItem(LogFilterItem logFilterItem)
        {
            _logFilterItems.Add(logFilterItem);
            logFilterItem.PropertyChanged += LogFilterItem_PropertyChanged;
            FilterChanged();
        }

        public Task EditFilterItem(LogFilterItem logFilterItem)
        {
            return _notify.ShowSubWindow(
                I18NKeys.Filter_Dialog_Edit.GetLocalizationRawValue(),
                new LogFilterItemEditorViewModel()
                {
                    LogFilterItem = logFilterItem
                });
        }

        public async Task<bool> RemoveFilterItem(LogFilterItem logFilterItem)
        {
            var content = I18NKeys.Common_ConfirmDelete.GetLocalizationString(logFilterItem.FilterKey);
            if (await _notify.ShowComfirmMessageBox(content))
            {
                _logFilterItems.Remove(logFilterItem);
                logFilterItem.PropertyChanged -= LogFilterItem_PropertyChanged;
                WeakReferenceMessenger.Default.Send(new LogFilterItemsChangedMessage(_logFilterItems));
                return true;
            }
            return false;
        }

        private void LogFilterItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LogFilterItem.Id) ||
                e.PropertyName == nameof(LogFilterItem.HexColor) ||
                e.PropertyName == nameof(LogFilterItem.Hits))
            {

            }
            else
            {
                FilterChanged();
            }
        }

        private void FilterChanged()
        {
            _debounceDispatcher.Debounce(200, async (x) =>
            {
                _ = Task.Run(() =>
                {
                    WeakReferenceMessenger.Default.Send(new LogFilterItemsChangedMessage(LogFilterItems));
                });
            });
        }

        public bool Search(string text, string keyword, bool matchCase, bool matchWholeWord, bool useRegex)
        {
            string pattern = keyword;
            if (!useRegex)
            {
                pattern = matchWholeWord ? $@"\b{Regex.Escape(keyword)}\b" : Regex.Escape(keyword);
            }

            return Regex.IsMatch(text, pattern, (matchCase | useRegex) ? RegexOptions.None : RegexOptions.IgnoreCase);
        }
    }
}
