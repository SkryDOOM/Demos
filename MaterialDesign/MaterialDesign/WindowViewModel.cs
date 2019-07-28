using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesign.Commands;
using MaterialDesign.Pages;

namespace MaterialDesign
{
    class WindowViewModel : BaseViewModel
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
        /// Min width of the window.
        /// </summary>
        public double WindowMinWidth { get; set; } = 500;

        /// <summary>
        /// Min height of the window.
        /// </summary>
        public double WindowMinHeight { get; set; } = 300;

        /// <summary>
        /// The actual width of the window.
        /// </summary>
        public double  WindowWidth { get; set; } = SystemParameters.PrimaryScreenWidth * 0.75;

        /// <summary>
        /// The actual height of the window.
        /// </summary>
        public double WindowHeight { get; set; } = SystemParameters.PrimaryScreenHeight * 0.67;
        
        /// <summary>
        /// Indicates which page is loaded currently.
        /// </summary>
        public Frames CurrentPage { get; set; } = Frames.ColorPage;

        #endregion

        #region Commands
        /// <summary>
        /// Command to close the entire program.
        /// </summary>
        public ICommand ExitWindowCommand { get; private set; }
       
        /// <summary>
        /// Command to set the window to fullscreen and back.
        /// </summary>
        public ICommand FullScreenWindowCommand { get; private set; }

        /// <summary>
        /// Command to minimize the window.
        /// </summary>
        public ICommand MinimizeWindowCommand { get; private set; }

        /// <summary>
        /// Enables windows to be dragged around.
        /// </summary>
        public ICommand DragWindowCommand { get; private set; }

        /// <summary>
        /// Changes the currently loaded page.
        /// </summary>
        public ICommand ChangePageCommand { get; private set; }
        #endregion

        #region Construtors
        public WindowViewModel(Window window)
        {
            mWindow = window;
            ExitWindowCommand = new RelayCommand(() => mWindow.Close());
            MinimizeWindowCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized );
            DragWindowCommand = new RelayCommand(() => mWindow.DragMove());
            FullScreenWindowCommand = new RelayCommand(SetFullscreenWindow);
            ChangePageCommand = new RelayParamCommand<TextBlock>(ChangePage);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the window to fullscreen according to the position of the taskbar and back.
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
                winState = WindowState.Normal;
                WindowWidth = SystemParameters.PrimaryScreenWidth * 0.75;
                WindowHeight = SystemParameters.PrimaryScreenHeight * 0.67;
                mWindow.Left = (SystemParameters.PrimaryScreenWidth - WindowWidth) / 2;
                mWindow.Top = (SystemParameters.PrimaryScreenHeight - WindowHeight) / 2;
            }
        }

        /// <summary>
        /// Change the current page.
        /// </summary>
        /// <param name="sender"></param>
        private void ChangePage(TextBlock sender)
        {
            switch (sender.Text)
            {
                case "Colors":
                    CurrentPage = Frames.ColorPage;
                    break;
                case "Fonts":
                    CurrentPage = Frames.FontPage;
                    break;
                default:
                    CurrentPage = Frames.IconPage;
                    break;
            }
        }

        #endregion
    }
}
