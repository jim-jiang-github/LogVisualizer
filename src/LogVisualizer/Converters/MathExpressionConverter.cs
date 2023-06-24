using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Converters
{
    public class MathExpressionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double doubleValue && parameter is string mathExpression)
            {
                try
                {
                    var expression = mathExpression.Replace("Value", doubleValue.ToString());
                    var expressionValue = new DataTable().Compute(expression, null);
                    if (double.TryParse(expressionValue.ToString(), out double converedValue))
                    {
                        return converedValue;
                    }
                    return value;
                }
                catch (Exception ex)
                {
                    Log.Warning("Math expression calculation error: " + ex.Message);
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
