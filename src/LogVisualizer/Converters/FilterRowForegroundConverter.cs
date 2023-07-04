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
    public class FilterRowForegroundConverter : IValueConverter
    {
        private FilterService? _filterService;

        public FilterRowForegroundConverter()
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
                        return Brushes.White;
                    }
                    if (Color.TryParse(logFilterItem.HexColor, out var color))
                    {
                        SolidColorBrush _brush = new(InvertColor(color));
                        return _brush;
                    }
                }
                catch (Exception)
                {
                    return Brushes.White;
                }
            }
            return Brushes.White;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }

        private Color InvertColor(Color color)
        {
            double brightness = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            return brightness > 0.5 ? Colors.Black : Colors.White;
        }
    }
}
