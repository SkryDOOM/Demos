using System;
using System.Globalization;
using System.Windows;

namespace EventAssistant.Converters
{
    /// <summary>
    /// Converts an int value to visibility.
    /// </summary>
    class IntToVisibilityConverter : BaseValueConverter<IntToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
