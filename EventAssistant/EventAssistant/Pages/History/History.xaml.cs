using System.Windows.Controls;

namespace EventAssistant
{
    public partial class History : Page
    {
        public History()
        {
            InitializeComponent();
            DataContext = new HistoryViewModel();
        }

        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
