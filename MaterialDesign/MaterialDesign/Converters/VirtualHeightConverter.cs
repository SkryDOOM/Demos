using System;
using System.Globalization;

namespace MaterialDesign.Converters
{
    class VirtualHeightConverter : BaseValueConverter<VirtualHeightConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value) || (double)value == 0)
                return 0;

            int actualHeight = ((System.Convert.ToInt32(value) - 100) / 225 * 226);

            return actualHeight;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
