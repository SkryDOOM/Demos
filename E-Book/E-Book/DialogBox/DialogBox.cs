using System.Windows;
using System.Windows.Controls;

namespace E_Book
{
    public partial class DialogBox : Window
    {
        #region Properties
        /// <summary>
        /// Contains the user's answer.
        /// </summary>
        public DialogAnswer Answer { get; private set; }

        /// <summary>
        /// The message to be shown.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The header message.
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// Button1 text.
        /// </summary>
        public string ButtonText1 { get; private set; } = "Nem";

        /// <summary>
        /// Button2 text.
        /// </summary>
        public string ButtonText2 { get; private set; } = "Rendben";

        /// <summary>
        /// Button3 text.
        /// </summary>
        public string ButtonText3 { get; private set; } = "Igen";

        /// <summary>
        /// Yes button visibility.
        /// </summary>
        public Visibility YesVisibility { get; private set; } = Visibility.Collapsed;

        /// <summary>
        /// No button visibility.
        /// </summary>
        public Visibility NoVisibility { get; private set; } = Visibility.Collapsed;

        /// <summary>
        /// OkButton Visibility.
        /// </summary>
        public Visibility OkVisibility { get; private set; } = Visibility.Visible;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// Initializes a default INFORMATION dialog.
        /// </summary>
        public DialogBox()
        : this(DialogBoxType.INFORMATION, "SIKER!", "A művelet végrehajtása sikeres!") { }

        /// <summary>
        /// Initializes an INFORMATION dialog with a custom message.
        /// </summary>
        /// <param name="message"> The text of the message. </param>
        public DialogBox(string message)
        : this(DialogBoxType.INFORMATION, "SIKER!", message) { }

        /// <summary>
        /// Initializes an INFORMATION dialog with a custom header and message.
        /// </summary>
        /// <param name="header"> The text of the header. </param>
        /// <param name="message"> the text of the message. </param>
        public DialogBox(string header, string message)
        : this(DialogBoxType.INFORMATION, header, message) { }

        /// <summary>
        /// Initializes the dialog with a custom type, header and message.
        /// </summary>
        /// <param name="type"> Type of the dialog. </param>
        /// <param name="message"> The text of the message. </param>
        public DialogBox(DialogBoxType type, string header, string message)
        {
            InitializeComponent();
            DataContext = this;

            Header = header;
            Message = message;

            if (type != DialogBoxType.INFORMATION)
            {
                YesVisibility = Visibility.Visible;
                NoVisibility = Visibility.Visible;
                OkVisibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enables the MessageBox to drag around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Responding according to the given answer.
        /// Sets the Answer value, or closes if it's an INFORMATION box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnswerPressed(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.Equals(ButtonText3))
                Answer = DialogAnswer.YES;
            else
                Answer = DialogAnswer.NO;

            this.Close();
        }
        #endregion
    }
}
