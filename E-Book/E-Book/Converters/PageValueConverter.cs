using E_Book.InterfacePages;
using System;
using System.Diagnostics;
using System.Globalization;

namespace E_Book.Converters
{
    class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((PagesEnum)value)
            {
                case PagesEnum.ADMINISTRATION:
                    return new AdministrationPage();

                case PagesEnum.BORROW:
                    return new BorrowPage();

                case PagesEnum.LIBRARY:
                    return new LibraryStorage();

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
