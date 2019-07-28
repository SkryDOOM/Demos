using System;
using System.Diagnostics;
using System.Globalization;

namespace EventAssistant
{
    /// <summary>
    /// Convert enum type to the given page.
    /// </summary>
    class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((PagesEnum)value)
            {
                case PagesEnum.TODAY:
                    return new HomePage();

                case PagesEnum.UPCOMING:
                    return new Upcoming();

                case PagesEnum.HISTORY:
                    return new History();

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
