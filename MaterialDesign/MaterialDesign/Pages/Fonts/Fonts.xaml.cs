using System.Windows;
using System.Windows.Controls;

namespace MaterialDesign.Pages
{
    public partial class Fonts : Page
    {
   
        public Fonts()
        {
            InitializeComponent();
            DataContext = new FontViewModel();
        }

        /// <summary>
        /// Switch back to detailed view if an item was pressed in the view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailedView(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CompareGrid.Visibility == Visibility.Visible)
            {
                CompareGrid.Visibility = Visibility.Hidden;
                DetailedGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
