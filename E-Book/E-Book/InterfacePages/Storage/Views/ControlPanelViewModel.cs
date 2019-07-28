using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using E_Book.Database;

namespace E_Book.InterfacePages
{
    class ControlPanelViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Displays the number of the available copies.
        /// </summary>
        public int Available { get; private set; }

        private Book mBook;
        /// <summary>
        /// contains the selected book which was selected on the storage datagrid.
        /// </summary>
        public Book SelectedBookDetails
        {
            get
            {
                Available = DetailsList.Where(r => r.IsAvailable.Equals("Igen")).Count();
                return mBook;
            }
            set { mBook = value; }
        }

        /// <summary>
        /// Contains the number of copies of the SelectedBookDetails property.
        /// </summary>
        public ObservableCollection<DetailsModel> DetailsList { get; set; }

        /// <summary>
        /// Contains the details of the new copies.
        /// </summary>
        public ObservableCollection<CopyModel> CopiesList { get; set; }

        /// <summary>
        /// Contains the new book that will be added to the library.
        /// </summary>
        public Book NewBook { get; set; } = new Book() { Year = 2019 };

        private string mNumbers;
        /// <summary>
        /// Number of copies to add to the database.
        /// </summary>
        public string CopyNumbers
        {
            get { return mNumbers; }
            set
            {
                Regex regex = new Regex(@"[^0-9]");
                Match match = regex.Match(value);

                if (match.Success)
                    return;

                mNumbers = value;
            }
        }
        #endregion

        #region Constructors
        public ControlPanelViewModel()
        {
            CopiesList = new ObservableCollection<CopyModel>();
            DetailsList = new ObservableCollection<DetailsModel>();
        }
        #endregion
    }
}
