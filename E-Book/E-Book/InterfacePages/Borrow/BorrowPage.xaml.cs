using System.Windows.Controls;

namespace E_Book.InterfacePages
{
    public partial class BorrowPage : Page
    {
        public BorrowPage()
        {
            InitializeComponent();
            DataContext = new BorrowViewModel();
        }
    }
}
