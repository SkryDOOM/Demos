using System.Windows.Controls;
using System.Windows.Input;

namespace E_Book.InterfacePages
{
    public partial class AdministrationPage : Page
    {

        private AdministrationViewModel Vm = new AdministrationViewModel();

        public AdministrationPage()
        {
            InitializeComponent();
            DataContext = Vm;
        }

        private void CheckLibEvent(object sender, KeyEventArgs e)
        {
            if (Vm == null) return;

             Vm.CheckLibID(e);
        }
    }
}
