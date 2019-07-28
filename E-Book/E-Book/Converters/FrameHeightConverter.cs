using System;
using System.Globalization;


namespace E_Book.Converters
{
    class FrameHeightConverter : BaseValueConverter<FrameHeightConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (double.IsNaN((double)value) || (double)value == 0)
                return 0;

            double actualHeight = Math.Round((double)value - 70);

            return actualHeight;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
