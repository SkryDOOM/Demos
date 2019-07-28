using E_Book.InterfacePages;
using System.Windows;

namespace E_Book.InterfacePages
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel(this);
        }
    }
}
