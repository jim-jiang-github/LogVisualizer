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
        private LogProcessorService? _logProcessorService;
        private static SolidColorBrush _unFilteredBruesh = new SolidColorBrush(Colors.White, 0.5d);

        public FilterRowForegroundConverter()
        {
            _filterService = DependencyInjectionProvider.GetService<FilterService>();
            _logProcessorService = DependencyInjectionProvider.GetService<LogProcessorService>();
        }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (_filterService != null && _logProcessorService != null && value?.ToString() is string s && targetType == typeof(IBrush))
            {
                var showOnlyFilteredLine = !_logProcessorService.ShowOnlyFilteredLines;
                var hasFilterItem = _filterService.LogFilterItems.Any(f => f.Enabled);
                try
                {
                    var logFilterItem = _filterService.LogFilterItems
                    .Where(f => f.Enabled)
                    .FirstOrDefault(f => _filterService.Search(s, f.FilterKey, f.IsMatchCase, f.IsMatchWholeWord, f.IsUseRegularExpression));
                    if (logFilterItem == null)
                    {
                        if (showOnlyFilteredLine && hasFilterItem)
                        {
                            return _unFilteredBruesh;
                        }
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
                    if (showOnlyFilteredLine && hasFilterItem)
                    {
                        return _unFilteredBruesh;
                    }
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
