using E_Book.InterfacePages.Pages;
using System.Windows.Controls;

namespace E_Book.InterfacePages
{
    public partial class LibraryStorage : Page
    {
        public LibraryStorage()
        {
            InitializeComponent();
            DataContext = new LibraryViewModel();
        }
    }
}
