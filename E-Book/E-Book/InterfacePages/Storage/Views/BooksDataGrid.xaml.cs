using System.Windows.Controls;

namespace E_Book.InterfacePages
{
    public partial class BooksDataGrid : UserControl
    {
        public BooksDataGrid()
        {
            InitializeComponent();
        }

        private void ScrollToSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            BookGrid.UpdateLayout();
            BookGrid.ScrollIntoView(BookGrid.SelectedItem);
        }
    }
}
