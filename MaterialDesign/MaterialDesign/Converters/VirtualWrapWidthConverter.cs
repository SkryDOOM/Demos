using System;
using System.Globalization;

namespace MaterialDesign.Converters
{
    class VirtualWrapWidthConverter : BaseValueConverter<VirtualWrapWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value) || (double)value == 0)
                return 0;

            double actualWidth = Math.Round((double)value / 275) * 275 - 265;

            return actualWidth;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
