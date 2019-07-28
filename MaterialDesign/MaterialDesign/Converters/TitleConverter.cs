using System;
using System.Globalization;
using System.Windows;

namespace MaterialDesign.Converters
{
    class TitleConverter : BaseValueConverter<TitleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return (Visibility)value == Visibility.Visible ? "Selected Fonts" : "Font Details";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
