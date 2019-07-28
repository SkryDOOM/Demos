using System.Windows;
using System.Windows.Input;

namespace EventAssistant
{
    /// <summary>
    /// This class is used to manipulate the attributes of the mainwindow and load the desired pages.
    /// </summary>
    class MainWindowViewModel : BaseViewModel
    {
        #region Private properties
        /// <summary>
        /// The window this class controls.
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// Indicates the current windowstate.
        /// </summary>
        private WindowState winState = WindowState.Normal;
        #endregion

        #region Public properties
        /// <summary>
        /// Indicates which page is loaded.
        /// </summary>
        public PagesEnum CurrentPage { get; private set; }

        /// <summary>
        /// Min width of the window.
        /// </summary>
        public double WindowMinWidth { get; private set; } = 840;

        /// <summary>
        /// Min height of the window.
        /// </summary>
        public double WindowMinHeight { get; private set; } = 490;

        /// <summary>
        /// The actual width of the window.
        /// </summary>
        public double WindowWidth { get; set; } = SystemParameters.PrimaryScreenWidth * 0.65;

        /// <summary>
        /// The actual height of the window.
        /// </summary>
        public double WindowHeight { get; set; } = SystemParameters.PrimaryScreenHeight * 0.65;
        #endregion

        #region Commands
        public ICommand FullScreenWindowCommand { get; private set; }
        public ICommand CloseWindowCommand { get; private set; }
        public ICommand MinimizeWindowCommand { get; private set; }
        public ICommand DragWindowCommand { get; private set; }
        public ICommand ChangePageCommand { get; private set; }
        #endregion

        #region Constructors
        public MainWindowViewModel(Window window)
        {
            mWindow = window;
            CloseWindowCommand = new RelayCommand(() => { mWindow.Close(); });
            DragWindowCommand = new RelayCommand(() => { mWindow.DragMove(); });
            MinimizeWindowCommand = new RelayCommand(() => { mWindow.WindowState = WindowState.Minimized; });
            FullScreenWindowCommand = new RelayCommand(SetFullscreenWindow);
            ChangePageCommand = new RelayParamCommand<string>(ChangePage);
        }
        #endregion

        #region Methods 
        /// <summary>
        /// Loads the desired page.
        /// </summary>
        /// <param name="sender"> Name of the page to be loaded. </param>
        private void ChangePage(string sender)
        {
            switch (sender)
            {
                case "Today":
                    CurrentPage = PagesEnum.TODAY;
                    break;

                case "Upcoming":
                    CurrentPage = PagesEnum.UPCOMING;
                    break;

                default:
                    CurrentPage = PagesEnum.HISTORY;
                    break;
            }
        }

        /// <summary>
        /// Sets the window to fullscreen according to the position of the taskbar and back to normal.
        /// </summary>
        private void SetFullscreenWindow()
        {
            if (winState.Equals(WindowState.Normal))
            {
                // Checking which side the taskbar is on.
                if (SystemParameters.WorkArea.Left > 0)
                {
                    mWindow.Top = 0;
                    mWindow.Left = SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Width;
                }
                else if (SystemParameters.WorkArea.Top > 0)
                {
                    mWindow.Top = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;
                    mWindow.Left = 0;
                }
                else if (SystemParameters.WorkArea.Left == 0 && SystemParameters.WorkArea.Width < SystemParameters.PrimaryScreenWidth)
                {
                    mWindow.Top = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;
                    mWindow.Left = 0;
                }
                else
                {
                    mWindow.Top = 0;
                    mWindow.Left = 0;
                }

                winState = WindowState.Maximized;
                WindowWidth = SystemParameters.WorkArea.Width;
                WindowHeight = SystemParameters.WorkArea.Height;
            }
            else
            {
                // Set window back to normal size.
                winState = WindowState.Normal;
                WindowWidth = SystemParameters.PrimaryScreenWidth * 0.65;
                WindowHeight = SystemParameters.PrimaryScreenHeight * 0.65;
                mWindow.Left = (SystemParameters.PrimaryScreenWidth - WindowWidth) / 2;
                mWindow.Top = (SystemParameters.PrimaryScreenHeight - WindowHeight) / 2;
            }
        }
        #endregion
    }
}