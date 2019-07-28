using System.ComponentModel;

namespace E_Book
{
    /// <summary>
    /// Base ViewModel that fire propertychanged event.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when any child elements is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}