using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace E_Book.Converters
{
    /// <summary>
        /// This class allows direct XAML usage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        /// <summary>
        /// A single static instance of this value converter.
        /// </summary>
        private static T mConverter = null;
    
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Creates a new instance of whatever the type is passed.
            return mConverter ?? (mConverter = new T());
        }
    
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
