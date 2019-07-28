using E_Book.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace E_Book.InterfacePages.Pages
{
    class LibraryViewModel : BaseViewModel
    {
        #region Properties
        private string mSearchTerm;
        /// <summary>
        /// Contains the search term.
        /// </summary>
        public string SearchTerm
        {
            get { return mSearchTerm; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    IsCollapsed = true;

                else if (mFlag == false)
                {
                    // Search for a matching item.
                    Regex regex = new Regex(value.ToLower());
                    var Result = BooksDataContext.StoredBooksList.Where(r => regex.IsMatch(r.BookName.ToLower())).ToList();
                    AutoFillList = new ObservableCollection<Book>(Result);

                    IsCollapsed = false;
                }

                mSearchTerm = value;
            }
        }

        /// <summary>
        /// Helper property to hide or show the autofill box.
        /// </summary>
        public bool IsCollapsed { get; set; } = true;

        private Book mSelected;
        /// <summary>
        /// Contains the selected autofill item.
        /// </summary>
        public Book SelectedAutoFillItem
        {
            get { return mSelected; }
            set
            {
                if (value != null)
                {
                    mFlag = true;
                    SearchTerm = value.BookName;
                    BooksDataContext.SelectedBook = value;
                }

                mFlag = false;
                IsCollapsed = true;
                mSelected = null;
            }
        }

        /// <summary>
        /// Indicates if an AutoFillItem was selected.
        /// </summary>
        private bool mFlag;

        /// <summary>
        /// Datacontext of the BooksDatagrid.xaml.
        /// </summary>
        public static BooksDataGridViewModel BooksDataContext { get; private set; }

        /// <summary>
        /// The datacontext of the controlpanel.xaml.
        /// </summary>
        public static ControlPanelViewModel ControlDataContext { get; private set; }

        /// <summary>
        /// Contains the matches of the search.
        /// </summary>
        public ObservableCollection<Book> AutoFillList { get; set; }

        /// <summary>
        /// Basic DialogBox to inform user.
        /// </summary>
        private DialogBox dialogBox;
        #endregion

        #region Commands
        public ICommand GetDetailsCommand { get; private set; }
        public ICommand AddNewBookCommand { get; private set; }
        public ICommand GenerateCopiesCommand { get; private set; }
        public ICommand AddNewCopiesCommand { get; private set; }
        #endregion

        #region Constructor
        public LibraryViewModel()
        {
            AutoFillList = new ObservableCollection<Book>();
            BooksDataContext = new BooksDataGridViewModel();
            ControlDataContext = new ControlPanelViewModel();
            GetDetailsCommand = new RelayParamCommand<Book>(GetDetails);
            AddNewBookCommand = new RelayParamCommand<Book>(AddNewBook);
            GenerateCopiesCommand = new RelayParamCommand<string>(GenerateCopies);
            AddNewCopiesCommand = new RelayCommand(AddNewCopies);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the details about the selected book and displays on the ControlPanel.xaml.
        /// </summary>
        /// <param name="sender"> The selected book </param>
        private void GetDetails(Book sender)
        {
            ControlDataContext.DetailsList = new ObservableCollection<DetailsModel>();            
            using (var context = new LibraryDatabaseEntities())
            {
                var item = context.BookCopies.Where(r => r.BookID.Equals(sender.ID)).ToList();
                foreach (var query in item)
                {
                    ControlDataContext.DetailsList.Add(new DetailsModel { BookCode = query.BookCode, IsAvailable = query.IsBorrowed == false ? "Igen" : "Nem" });
                }
            }
            ControlDataContext.SelectedBookDetails = sender;
        }

        /// <summary>
        /// Add a new book to the database.
        /// </summary>
        /// <param name="sender">The details of the new book </param>
        private void AddNewBook(Book sender)
        {
            if (sender.Year == 0 || string.IsNullOrWhiteSpace(sender.BookName) || string.IsNullOrWhiteSpace(sender.Authors) || string.IsNullOrWhiteSpace(sender.Category))
            {
                dialogBox = new DialogBox("FIGYELEM!","Minden mező kitöltése kötelező!");
                dialogBox.Show();
                return;
            }

            dialogBox = new DialogBox(DialogBoxType.WARNING,"VÉGLEGESÍTÉS!","Könyv hozzáadása?");
            dialogBox.ShowDialog();
            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new LibraryDatabaseEntities())
            {
                try
                {
                    context.Books.Add(sender);
                    context.SaveChanges();
                    BooksDataContext.StoredBooksList.Add(sender);
                    dialogBox = new DialogBox("Hozzáadás sikeres!");
                    dialogBox.Show();
                }
                catch
                {
                    dialogBox = new DialogBox("HIBA!","Hozzáadás nem sikerült!");
                    dialogBox.Show();
                }
            }
            ControlDataContext.SelectedBookDetails = sender;
            
        }

        /// <summary>
        /// Create the copies controls.
        /// </summary>
        /// <param name="sender"> The number of the copies control to create. </param>
        private void GenerateCopies(string sender)
        {
            if (string.IsNullOrWhiteSpace(sender))
                return;

            int nCopies = int.Parse(sender);
            int copyListSize = ControlDataContext.CopiesList.Count;

            if (nCopies > copyListSize)
            {
                for (int i = copyListSize; i < nCopies; i++)
                {
                    ControlDataContext.CopiesList.Add(new CopyModel(i + 1));
                }
            }
            else if (nCopies < copyListSize)
            {
                for (int i = copyListSize; i > nCopies; i--)
                {
                    ControlDataContext.CopiesList.RemoveAt(i - 1);
                }
            }
        }

        /// <summary>
        /// Add the copies to the database.
        /// </summary>
        private void AddNewCopies()
        {
            if (ControlDataContext.SelectedBookDetails == null)
                return;

            foreach (var item in ControlDataContext.CopiesList)
            {
                if (item.BookCode.Length != 6)
                {
                    dialogBox = new DialogBox("HIBA!","Hibásan kitöltött mező!");
                    dialogBox.Show();
                    return;
                }
            }

            dialogBox = new DialogBox(DialogBoxType.WARNING, "FIGYELEM!","Hozzáadja a másolatokat?");
            dialogBox.ShowDialog();
            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new LibraryDatabaseEntities())
            {
                foreach (var item in ControlDataContext.CopiesList)
                {
                    var bookCopy = new BookCopy
                    {
                        BookCode = item.BookCode.ToUpper(),
                        BookID = ControlDataContext.SelectedBookDetails.ID
                    };

                    context.BookCopies.Add(bookCopy);
                }

                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    dialogBox = new DialogBox("HIBA!", "Már létezik ilyen azonosító!\nHozzáadás nem lehetséges!");
                    dialogBox.Show();
                    return;
                }

                // Calculate the new value of the available copies.

                short count = (short)context.BookCopies.Count(t => t.BookID == ControlDataContext.SelectedBookDetails.ID);
                var books = context.Books.FirstOrDefault(r => r.ID == ControlDataContext.SelectedBookDetails.ID);
                books.BookCopies = count;
                context.SaveChanges();

                //Update thhe copies column acording to the new value.
                 BooksDataContext.StoredBooksList
                .Where(r => r.ID == ControlDataContext.SelectedBookDetails.ID)
                .Select(n => { n.BookCopies = count; return n; }).ToList();
            }
        }
        #endregion
    }
}
