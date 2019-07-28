using System.Windows;
using System.Windows.Controls;

namespace MaterialDesign.Pages
{
    /// <summary>
    /// Interaction logic for Colors.xaml
    /// </summary>
    public partial class Colors : Page
    {
        public Colors()
        {
            InitializeComponent();
            DataContext = new ColorViewModel();
        }

        /// <summary>
        /// Scrolls to the top of the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorScroll.ScrollToTop();
        }

        /// <summary>
        /// Allows the user to scroll with the mousewheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}