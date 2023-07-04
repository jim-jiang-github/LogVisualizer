using Avalonia.Data.Converters;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using ReverseMarkdown.Converters;
using LogVisualizer.Services;

namespace LogVisualizer.Converters
{
    public class FilterRowBackgroundConverter : IValueConverter
    {
        private FilterService? _filterService;

        public FilterRowBackgroundConverter()
        {
            _filterService = DependencyInjectionProvider.GetService<FilterService>();
        }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (_filterService != null && value?.ToString() is string s && targetType == typeof(IBrush))
            {
                try
                {
                    var logFilterItem = _filterService.LogFilterItems
                    .Where(f => f.Enabled)
                    .FirstOrDefault(f => _filterService.Search(s, f.FilterKey, f.IsMatchCase, f.IsMatchWholeWord, f.IsUseRegularExpression));
                    if (logFilterItem == null)
                    {
                        return Brushes.Transparent;
                    }
                    if (Color.TryParse(logFilterItem.HexColor, out var color))
                    {
                        SolidColorBrush _brush = new(color);
                        return _brush;
                    }
                }
                catch (Exception)
                {
                    return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
