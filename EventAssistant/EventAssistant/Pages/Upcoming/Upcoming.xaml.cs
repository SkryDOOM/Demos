using EventAssistant.Database;
using System.Windows.Controls;

namespace EventAssistant
{
    public partial class Upcoming : Page
    {
        public Upcoming()
        {
            InitializeComponent();
            DataContext = new UpComingViewModel();
        }

        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
