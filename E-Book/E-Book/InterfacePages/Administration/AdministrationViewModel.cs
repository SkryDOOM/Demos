using E_Book.Database;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace E_Book.InterfacePages
{
    class AdministrationViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Contains the details about a member.
        /// </summary>
        public Member MemberInfo { get; set; }

        /// <summary>
        /// Contains the length of the selected membership.
        /// </summary>
        private string MembershipLength;

        /// <summary>
        /// Controls the availability of the controls.
        /// </summary>
        public Visibility IsEnabled { get; private set; } = Visibility.Visible;

        /// <summary>
        /// Datatrigger for the register button.
        /// </summary>
        public bool IsRegisterClicked { get; private set; } = true;

        /// <summary>
        /// Datatrigger for the expand button.
        /// </summary>
        public bool IsExpandClicked { get; private set; } = false;

        /// <summary>
        /// Displays a Check icon if the given ID exists.
        /// </summary>
        public Visibility IsExist { get; private set; } = Visibility.Collapsed;

        /// <summary>
        /// Basic dialogbox to inform user.
        /// </summary>
        private DialogBox dialogBox;
        #endregion

        #region Commands
        public ICommand ConfirmCommand { get; private set; }
        public ICommand RadioButtonCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand ExpandCommand { get; private set; }
        #endregion

        #region Constructors
        public AdministrationViewModel()
        {
            MemberInfo = new Member() { DateOfBirth = DateTime.Today};
            RadioButtonCommand = new RelayParamCommand<string>((sender) => MembershipLength = sender.Split(' ')[0]);
            ConfirmCommand = new RelayCommand(RegisterMember);
            RegisterCommand = new RelayCommand(RegisterClicked);
            ExpandCommand = new RelayCommand(ExpandClicked);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Register a new member or expand the membership. 
        /// </summary>
        private void RegisterMember()
        {
            if (IsRegisterClicked && !IsValidFields())
            {
               dialogBox = new DialogBox("HIBA!","Helytelenül kitöltött Mezők!");
               dialogBox.Show();
                return;
            }


            if (IsEnabled == Visibility.Visible)
            {
                dialogBox = new DialogBox(DialogBoxType.WARNING,"FIGYELEM!", "Regisztrálja az új tagot?");
                dialogBox.ShowDialog();
                if (dialogBox.Answer == DialogAnswer.NO)
                    return;

                // Register a new member.
                using (var context = new LibraryDatabaseEntities())
                {
                    try
                    {
                        var item = DateTime.Today;
                        item = item.AddMonths(int.Parse(MembershipLength));
                        MemberInfo.ExpData = item;
                        MemberInfo.LibID = MemberInfo.LibID.ToUpper();

                        context.Members.Add(MemberInfo);
                        context.SaveChanges();
                        dialogBox = new DialogBox("Sikeres regisztráció!");
                        dialogBox.Show();
                    }
                    catch
                    {
                        dialogBox = new DialogBox("HIBA!", "Művelet sikertelen!");
                        dialogBox.Show();
                        return;
                    }
                }
            }
            else
            {
                dialogBox = new DialogBox(DialogBoxType.WARNING,"FIGYELEM!", "Tagság meghosszabítása?");
                dialogBox.ShowDialog();
                if (dialogBox.Answer == DialogAnswer.NO)
                    return;

                // Expand the membership of an existing member.
                using (var context = new LibraryDatabaseEntities())
                {
                    var item = context.Members.FirstOrDefault(r => r.LibID.Equals(MemberInfo.LibID));

                    if (item == null)
                    {
                        dialogBox = new DialogBox("HIBA!", "Nem található ilyen felhasználó!");
                        dialogBox.Show();
                        return;
                    }

                    //Expand membership.
                    try
                    {
                        var dateCurrent = item.ExpData;
                        item.ExpData = dateCurrent.AddMonths(int.Parse(MembershipLength));

                        context.SaveChanges();

                        dialogBox = new DialogBox("SIKER!","Tagság sikeresen meghosszabbítva!");
                        dialogBox.Show();
                    }
                    catch
                    {
                        dialogBox = new DialogBox("HIBA!","Művelet sikertelen!");
                        dialogBox.Show();
                    }
                }
            }
        }

        /// <summary>
        /// Validate every field, and returns true if they're all correct otherwise return false.
        /// </summary>
        /// <returns></returns>
        private bool IsValidFields()
        {
            if (string.IsNullOrWhiteSpace(MemberInfo.FullName) || string.IsNullOrWhiteSpace(MemberInfo.PlaceOfBirth) || string.IsNullOrWhiteSpace(MemberInfo.MobilePhone)
               || string.IsNullOrWhiteSpace(MemberInfo.Email) || string.IsNullOrWhiteSpace(MemberInfo.LibID)
               || string.IsNullOrWhiteSpace(MemberInfo.Address) || string.IsNullOrEmpty(MembershipLength))
                return false;

            //#1: Full Name
            Regex regex = new Regex(@"[^\p{L} \.'\-]");
            Match match = regex.Match(MemberInfo.FullName);

            if(match.Success)
                return false;

            //#2: Place of Birth
            regex = new Regex(@"[^A-z|\-| ]");
            match = regex.Match(MemberInfo.PlaceOfBirth);

            if (match.Success)
                return false;

            //#3: Telephone
            regex = new Regex(@"\(?\d{2}\)?-? *\d{3}-? *-?\d{4}");
            match = regex.Match(MemberInfo.MobilePhone);

            if (!match.Success)
                return false;

            //#4: Email
            regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            match = regex.Match(MemberInfo.Email);

            if (!match.Success)
                return false;

            //#5: LibID
            regex = new Regex(@"^([A-z|0-9]){5}$");
            match = regex.Match(MemberInfo.LibID);

            if (!match.Success)
                return false;

            return true;
        }

        /// <summary>
        /// Switch to register menu.
        /// </summary>
        private void RegisterClicked()
        {
            IsEnabled = Visibility.Visible;
            IsExpandClicked = false;
            IsRegisterClicked = true;
        }

        /// <summary>
        /// Switch to Expand menu.
        /// </summary>
        private void ExpandClicked()
        {
            IsEnabled = Visibility.Collapsed;
            IsRegisterClicked = false;
            IsExpandClicked = true;
        }

        /// <summary>
        /// Check if the given library ID exists.
        /// </summary>
        /// <param name="e"></param>
        public void CheckLibID(KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MemberInfo.LibID))
                return;

            if (MemberInfo.LibID.Length != 5 || IsRegisterClicked == true)
            {
                IsExist = Visibility.Collapsed;
                return;
            }

            using (var context = new LibraryDatabaseEntities())
            {
                var item = context.Members.FirstOrDefault(r => r.LibID.Equals(MemberInfo.LibID));
                if (item == null)
                {
                    dialogBox = new DialogBox("HIBA!","Azonosító nem található!");
                    dialogBox.Show();
                    return;
                }

                IsExist = Visibility.Visible;
            }
        }
        #endregion
    }
}
