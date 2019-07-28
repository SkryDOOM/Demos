using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using E_Book.Database;

namespace E_Book.InterfacePages
{
    class BorrowViewModel : BaseViewModel
    {
        #region Properties       
        /// <summary>
        /// Datacontext of the borrow interface.
        /// </summary>
        public BorrowInterfaceViewModel BorrowDataContext { get; set; }

        /// <summary>
        /// The Datacontext of the return interface.
        /// </summary>
        public ReturnInterfaceViewModel ReturnDataContext { get; set; }

        /// <summary>
        /// Datatrigger for the Borrow button.
        /// </summary>
        public bool IsRegisterClicked { get; private set; } = true;

        /// <summary>
        /// Datatrigger for the Return button.
        /// </summary>
        public bool IsExpandClicked { get; private set; } = false;

        /// <summary>
        /// Basic dialogbox to inform user.
        /// </summary>
        private DialogBox dialogBox;
        #endregion
        
        #region Commands
        public ICommand AddBookCommand { get; private set; }
        public ICommand ConfirmCommand { get; private set; }
        public ICommand BorrowCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand ReturnBookCommand { get; private set; }
        public ICommand SearchMemberCommand { get; private set; }
        public ICommand AddAllBookCommand { get; private set; }
        public ICommand AddToCheckCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        #endregion

        #region Construtors
        public BorrowViewModel()
        {
            BorrowDataContext = new BorrowInterfaceViewModel();
            ReturnDataContext = new ReturnInterfaceViewModel();

            AddBookCommand = new RelayCommand(AddBookEvent);
            ConfirmCommand = new RelayCommand(ConfirmEvent);
            BorrowCommand = new RelayCommand(BorrowClicked);
            ReturnCommand = new RelayCommand(ReturnClicked);
            ReturnBookCommand = new RelayCommand(ReturnBook);
            SearchMemberCommand = new RelayCommand(SearchMember);
            AddAllBookCommand = new RelayCommand(AddAllBook);
            AddToCheckCommand = new RelayCommand(AddToCheck);
            RemoveItemCommand = new RelayCommand(RemoveItem);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the changes.
        /// </summary>
        private void ConfirmEvent()
        {
            if (IsRegisterClicked)
                BorrowBooks();
            else
                ReturnBooks();
        }

        /// <summary>
        /// Switch to Borrow menu.
        /// </summary>
        private void BorrowClicked()
        {
            IsExpandClicked = false;
            IsRegisterClicked = true;
            BorrowDataContext.BorrowVisibility = Visibility.Visible;
            ReturnDataContext.ReturnVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Switch to Return menu.
        /// </summary>
        private void ReturnClicked()
        {
            IsRegisterClicked = false;
            IsExpandClicked = true;
            BorrowDataContext.BorrowVisibility = Visibility.Collapsed;
            ReturnDataContext.ReturnVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Add a book to the borrow list.
        /// </summary>
        private void AddBookEvent()
        {
            // Check if code exist.
            using (var context = new LibraryDatabaseEntities())
            {
                var item = context.BookCopies.FirstOrDefault(r => r.BookCode.Equals(BorrowDataContext.BookID));
                if (item != null)
                {
                    // Prevent duplicates.
                    var isDuplicated = BorrowDataContext.BorrowedBooksList.FirstOrDefault(r => r.CopyID.Equals(item.ID));
                    if (isDuplicated != null) return;

                    // Check if it's already borrowed.
                    var isBorrowed = context.BorrowedBooks.FirstOrDefault(r => r.CopyID == item.ID);
                    if (isBorrowed != null)
                    {
                        dialogBox = new DialogBox("HIBA!","Ez a könyv nem kölcsönözhető!");
                        dialogBox.Show();
                        return;
                    }

                    // Add the copy to the borrow list.
                    var book = context.Books.FirstOrDefault(r => r.ID.Equals(item.BookID));
                    BorrowDataContext.BorrowedBooksList.Add(new BorrowModel() { CopyID = item.ID, BookCode = BorrowDataContext.BookID, BookName = book.BookName });
                }
                else
                {
                    dialogBox = new DialogBox("HIBA!", "A megadott könyv nem található!");
                    dialogBox.Show();
                }
            }
        }

        /// <summary>
        /// Add a book to the ReturnedBooksList.
        /// </summary>
        private void ReturnBook()
        {
            if (string.IsNullOrWhiteSpace(ReturnDataContext.ReturnBookCode))
                return;

            //Prevent duplicates.
            var item = ReturnDataContext.ReturnedBooksList.FirstOrDefault(r => r.CopyID.Equals(ReturnDataContext.ReturnBookCode));
            if (item != null)
                return;

            using (var context = new LibraryDatabaseEntities())
            {
                try
                {
                    //#1: Book copy details.
                    var bookCopy = context.BookCopies.FirstOrDefault(r => r.BookCode.Equals(ReturnDataContext.ReturnBookCode));

                    //#2: Book title.
                    var bookTitle = context.Books.FirstOrDefault(r => r.ID.Equals(bookCopy.BookID));

                    //#3: Borrow details.
                    var borrowed = context.BorrowedBooks.FirstOrDefault(r => r.CopyID.Equals(bookCopy.ID));

                    //#4: Member details
                    var member = context.Members.FirstOrDefault(r => r.ID.Equals(borrowed.LibID));

                    ReturnDataContext.ReturnedBooksList.Add(new ReturnModel()
                    {
                        BookName = bookTitle.BookName,
                        CopyID = bookCopy.BookCode,
                        ExpDate = borrowed.ExpDate,
                        MemberName = member.FullName,
                        MemberID = member.LibID
                    });
                }
                catch
                {
                    dialogBox = new DialogBox("HIBA!", "Nincs ilyen kiadott könyv!");
                    dialogBox.Show();
                }
            }
        }

        /// <summary>
        /// Get every borrowed book of a member.
        /// </summary>
        private void SearchMember()
        {
            if (string.IsNullOrWhiteSpace(ReturnDataContext.MemberID))
                return;

            using (var context = new LibraryDatabaseEntities())
            {
                var item = context.Members.FirstOrDefault(r => r.LibID.Equals(ReturnDataContext.MemberID));

                if (item == null)
                {
                    dialogBox = new DialogBox("HIBA!","Azonosító nem található!");
                    dialogBox.Show();
                    return;
                }

                //Reset the list.
                ReturnDataContext.MemberBooksList = new ObservableCollection<ReturnModel>();

                // List of every borrowed book by a member.
                var books = context.BorrowedBooks.Where(r => r.LibID.Equals(item.ID)).ToList();

                // Get the name and the copy code of the borrowed book.
                foreach (var collection in books)
                {
                    var bookItem = (from book in context.Books
                                   join copies in context.BookCopies
                                   on book.ID equals copies.BookID
                                   join borrow in context.BorrowedBooks
                                   on copies.ID equals borrow.CopyID
                                   where copies.ID == collection.CopyID
                                   select new { Book = book }).Single();

                    var copyItem = (from copies in context.BookCopies
                                   join borrowed in context.BorrowedBooks
                                   on copies.ID equals borrowed.CopyID
                                   where copies.ID == collection.CopyID
                                   select new { BookCopy = copies }).Single();

                    ReturnDataContext.MemberBooksList.Add(new ReturnModel
                    {
                        BookName = bookItem.Book.BookName,
                        CopyID = copyItem.BookCopy.BookCode,
                        MemberName = item.FullName,
                        MemberID = item.LibID,
                        ExpDate = collection.ExpDate
                    });    
                }
            }
        }

        /// <summary>
        /// Add every book that a man borrowed to the check list.
        /// </summary>
        private void AddAllBook()
        {
            foreach (var item in ReturnDataContext.MemberBooksList)
            {
                var isExist = ReturnDataContext.ReturnedBooksList.FirstOrDefault(r => r.CopyID.Equals(item.CopyID));

                if (isExist == null)
                    ReturnDataContext.ReturnedBooksList.Add(item);
            }
        }

        /// <summary>
        ///  Add the selected item on the member datagrid to the check datagrid.
        /// </summary>
        private void AddToCheck()
        {
            var item = ReturnDataContext.ReturnedBooksList.FirstOrDefault(r => r.CopyID.Equals(ReturnDataContext.SelectedMemberItem.CopyID));
            if (item == null)
                ReturnDataContext.ReturnedBooksList.Add(ReturnDataContext.SelectedMemberItem);
        }

        /// <summary>
        /// Remove the selected item from the check datagrid.
        /// </summary>
        private void RemoveItem()
        {
            ReturnDataContext.ReturnedBooksList.Remove(ReturnDataContext.SelectedCheckItem);
        }

        /// <summary>
        /// Borrow the books and add the selected books to the borrowed table.
        /// </summary>
        private void BorrowBooks()
        {
            //Check empty fields
            if (BorrowDataContext.BorrowedBooksList == null)
            {
                dialogBox = new DialogBox("HIBA!", "Nem adott hozzá könyvet!");
                dialogBox.Show();
                return;
            }

            if (BorrowDataContext.IsExpired || BorrowDataContext.IsExist == false)
            {
                dialogBox = new DialogBox("HIBA!", "Hibás vagy lejárt azonosító!");
                dialogBox.Show();
                return;
            }

            if (BorrowDataContext.ExpDate < DateTime.Today)
            {
                dialogBox = new DialogBox("HIBA!", "Érvénytelen kölcsönzési idő");
                dialogBox.Show();
                return;
            }

            dialogBox = new DialogBox(DialogBoxType.WARNING, "FIGYELEM!", "Művelet megerősítése?");
            dialogBox.ShowDialog();
            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new LibraryDatabaseEntities())
            {
                foreach (var item in BorrowDataContext.BorrowedBooksList)
                {
                    context.BorrowedBooks.Add(new BorrowedBook { LibID = BorrowDataContext.LibID, ExpDate = BorrowDataContext.ExpDate, CopyID = item.CopyID });
                    var bookCopies = context.BookCopies.FirstOrDefault(r => r.ID == item.CopyID);
                    bookCopies.IsBorrowed = true;
                }

                context.SaveChanges();
                dialogBox = new DialogBox("Kölcsönzés sikeres!");
                dialogBox.Show();

                // Reset the properties and list.
                BorrowDataContext.BorrowedBooksList = new ObservableCollection<BorrowModel>();
                BorrowDataContext.BookID = "";
                BorrowDataContext.MemberID = "";

            }
        }

        /// <summary>
        /// Return the borrowed books to the library and remove them from the borrowed table.
        /// </summary>
        private void ReturnBooks()
        {
            if (ReturnDataContext.ReturnedBooksList.Count == 0)
                return;

            dialogBox = new DialogBox(DialogBoxType.WARNING,"FIGYELEM!","Biztos visszaadja a könyveket?");
            dialogBox.ShowDialog();
            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new LibraryDatabaseEntities())
            {
                try
                {
                    foreach (var item in ReturnDataContext.ReturnedBooksList)
                    {
                        var returnBook = (from borrowed in context.BorrowedBooks
                                         join copies in context.BookCopies
                                         on borrowed.CopyID equals copies.ID
                                         where copies.BookCode == item.CopyID
                                         select new { BorrowedBook = borrowed }).Single().BorrowedBook;

                       context.BorrowedBooks.Remove(returnBook);
                    }

                    context.SaveChanges();
                    dialogBox = new DialogBox("Könyvek visszaadása sikeres!");
                    dialogBox.Show();
                    ReturnDataContext.ReturnedBooksList = new ObservableCollection<ReturnModel>();
                    ReturnDataContext.MemberBooksList = new ObservableCollection<ReturnModel>();
                }
                catch
                {
                    dialogBox = new DialogBox("HIBA!","Hiba a művelet közben!");
                    dialogBox.Show();
                }
            }
        }
        #endregion
    }
}
