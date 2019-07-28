using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace E_Book.InterfacePages
{
    class ReturnInterfaceViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// The visibility of the return interface.
        /// </summary>
        public Visibility ReturnVisibility { get; set; } = Visibility.Collapsed;

        private string mCode;
        /// <summary>
        /// The 6 character length book identifier.
        /// </summary>
        public string ReturnBookCode
        {
            get { return mCode; }
            set
            {
                Regex regex = new Regex(@"[^A-z0-9]");
                Match match = regex.Match(value);

                if (match.Success)
                    return;

                mCode = value.ToUpper();
            }
        }

        private string mMemberID;
        /// <summary>
        /// The 5 character length member identifier.
        /// </summary>
        public string MemberID
        {
            get { return mMemberID; }
            set
            {
                Regex regex = new Regex(@"[^A-z0-9]");
                Match match = regex.Match(value);

                if (match.Success)
                    return;

                mMemberID = value.ToUpper();
            }
        }

        /// <summary>
        /// Contains the returned books.
        /// </summary>
        public ObservableCollection<ReturnModel> ReturnedBooksList { get; set; }

        /// <summary>
        /// Contains every book that a member borrowed.
        /// </summary>
        public ObservableCollection<ReturnModel> MemberBooksList { get; set; }

        /// <summary>
        /// The selected member item on the member datagrid.
        /// </summary>
        public ReturnModel SelectedMemberItem { get; set; }

        /// <summary>
        /// The selected item on the check gridview.
        /// </summary>
        public ReturnModel SelectedCheckItem { get; set; }
        #endregion

        #region Constructors
        public ReturnInterfaceViewModel()
        {
            ReturnedBooksList = new ObservableCollection<ReturnModel>();
            MemberBooksList = new ObservableCollection<ReturnModel>();
        }
        #endregion
    }
}
