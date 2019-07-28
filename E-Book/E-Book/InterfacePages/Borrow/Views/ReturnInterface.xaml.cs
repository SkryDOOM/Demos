using System.Windows;
using System.Windows.Controls;

namespace E_Book.InterfacePages
{
    public partial class ReturnInterface : UserControl
    {
        public ReturnInterface()
        {
            InitializeComponent();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            CloseMember.Visibility = Visibility.Visible;
            OpenMember.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            CloseMember.Visibility = Visibility.Collapsed;
            OpenMember.Visibility = Visibility.Visible;
        }
    }
}
