using E_Book.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace E_Book.InterfacePages
{
    class BooksDataGridViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// List of every available book in the library.
        /// </summary>
        public ObservableCollection<Book> StoredBooksList { get; set; }

        /// <summary>
        /// Selected book in the gridview.
        /// </summary>
        public Book SelectedBook { get; set; }
        #endregion

        #region Contrustors
        public BooksDataGridViewModel()
        {
            using (var context = new LibraryDatabaseEntities())
            {
                var book = context.Books.ToList();
                StoredBooksList = new ObservableCollection<Book>(book);
            }
        }
        #endregion
    }
}
