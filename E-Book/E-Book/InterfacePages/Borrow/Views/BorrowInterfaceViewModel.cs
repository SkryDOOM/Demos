using E_Book.Database;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace E_Book.InterfacePages
{
    class BorrowInterfaceViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Contains the unique ID of the book from the BookCopiesTable.
        /// </summary>
        public string BookID { get; set; }

        /// <summary>
        /// Contains the list of the borrowed book(s).
        /// </summary>
        public ObservableCollection<BorrowModel> BorrowedBooksList { get; set; }

        /// <summary>
        /// The selected combobox item.
        /// </summary>
        public BorrowModel SelectedBook { get; set; }

        /// <summary>
        /// The ID of the member.
        /// </summary>
        public int LibID { get; private set; }

        private string mMemberID;
        /// <summary>
        /// Contains the unique ID of a member.
        /// </summary>
        public string MemberID
        {
            get { return mMemberID; }

            set
            {
                IsExist = false;
                IsExpired = false;

                if (value.Length == 5)
                {
                    using (var context = new LibraryDatabaseEntities())
                    {
                        var rep = context.Members.FirstOrDefault(r => r.LibID.Equals(value));

                        // Return a check icon if id was found, and displays an error message if membership expired.
                        if (rep != null)
                        {
                            IsExist = true;
                            if (rep.ExpData <= ExpDate)
                                IsExpired = true;
                            LibID = rep.ID;
                        }
                        else
                        {
                            dialogBox = new DialogBox("Érvénytelen azonosító!");
                            dialogBox.Show();
                        }
                    }
                }
                mMemberID = value;
            }
        }

        /// <summary>
        /// Basic DialogBox to inform user.
        /// </summary>
        private DialogBox dialogBox;

        /// <summary>
        /// Contains the date when the book(s) must be returned.
        /// </summary>
        public DateTime ExpDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The visibility of the borrow interface.
        /// </summary>
        public Visibility BorrowVisibility { get; set; } = Visibility.Visible;

        public bool IsExpired { get; set; }

        /// <summary>
        /// Displays a check icon if library ID exists.
        /// </summary>
        public bool IsExist { get; private set; }
        #endregion

        #region Commands
        /// <summary>
        /// Removes the selected item from the combobox.
        /// </summary>
        public ICommand DeleteItemCommand { get; set; }
        #endregion

        #region Constructor
        public BorrowInterfaceViewModel()
        {
            DeleteItemCommand = new RelayCommand(() => BorrowedBooksList.Remove(SelectedBook));
            BorrowedBooksList = new ObservableCollection<BorrowModel>();
        }
        #endregion
    }
}
