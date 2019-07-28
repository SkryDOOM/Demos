using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace EventAssistant
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = new HomePageViewModel();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            CloseEventButton.Visibility = Visibility.Visible;
            OpenEventButton.Visibility = Visibility.Collapsed;
            TodayEvents.Visibility = Visibility.Collapsed;
            AddStack.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            CloseEventButton.Visibility = Visibility.Collapsed;
            OpenEventButton.Visibility = Visibility.Visible;            
            TodayEvents.Visibility = Visibility.Visible;
            AddStack.Visibility = Visibility.Collapsed;
        }
    }
}
