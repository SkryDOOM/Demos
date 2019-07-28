using System;
using System.Globalization;

namespace EventAssistant.Converters
{
    /// <summary>
    /// Converter to invert a bool value.
    /// </summary>
    class BoolInverterConverter : BaseValueConverter<BoolInverterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? false : true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
