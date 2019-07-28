using System;
using System.Globalization;

namespace MaterialDesign.Converters
{
    class FontListHeightConverter : BaseValueConverter<FontListHeightConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value) || (double)value == 0)
                return 0;

            double actualHeight = Math.Round((double)value) - 135;

            return actualHeight;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
