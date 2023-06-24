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

namespace LogVisualizer.Converters
{
    public class HexInvertBrushConverter : IValueConverter
    {
        private SolidColorBrush _brush = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string s && targetType == typeof(IBrush))
            {
                try
                {
                    if (Color.TryParse(s, out var color))
                    {
                        SolidColorBrush _brush = new(InvertColor(color));
                        return _brush;
                    }
                }
                catch (Exception)
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush && targetType == typeof(string))
            {
                try
                {
                    return $"#{ToUInt32(InvertColor(brush.Color)):x8}";
                }
                catch (Exception)
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }

        private uint ToUInt32(Color color)
        {
            return ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | (uint)color.B;
        }

        private Color InvertColor(Color color)
        {
            double brightness = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            return brightness > 0.5 ? Colors.Black : Colors.White;
        }
    }
}
