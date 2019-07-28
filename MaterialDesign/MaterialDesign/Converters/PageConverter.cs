using System;
using System.Globalization;
using MaterialDesign.Pages;
using System.Diagnostics;

namespace MaterialDesign.Converters
{
    class PageConverter : BaseValueConverter<PageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Frames)value)
            {
                case Frames.ColorPage:
                    return new Colors();

                case Frames.FontPage:
                    return new Fonts();

                case Frames.IconPage:
                    return new Icons();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
