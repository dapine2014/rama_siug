using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SIUGJ.Services
{
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return ConvertInternal(value, parameter);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return ConvertInternal(value, parameter);
        }

        private static bool ConvertInternal(object? value, object? parameter)
        {
            bool inverse = parameter as string == "Inverse";
            bool boolValue = value as bool? ?? false;
            return inverse ? !boolValue : boolValue;
        }
    }
}
