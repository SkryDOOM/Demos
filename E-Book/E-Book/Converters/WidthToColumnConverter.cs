using System;
using System.Globalization;

namespace E_Book.Converters
{
    class WidthToColumnConverter : BaseValueConverter<WidthToColumnConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value) || (double)value == 0)
                return 0;

            int columns = (int)Math.Floor((double)value / 110);

            return columns;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
