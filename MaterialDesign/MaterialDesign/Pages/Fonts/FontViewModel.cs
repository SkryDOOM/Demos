using MaterialDesign.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialDesign.Pages
{
    class FontViewModel : BaseViewModel
    {
        #region properties
        /// <summary>
        /// Collection of the system fonts.
        /// </summary>
        public ObservableCollection<FontModel> SystemFontsList { get; private set; }

        /// <summary>
        /// Contains the font names that should be compared.
        /// </summary>
        public ObservableCollection<DetailedFonts> ComparedFontsList { get; private set; }

        /// <summary>
        /// Contains the variants of the selected font type.
        /// </summary>
        public FontModel SelectedFont { get; set; }

        private string mFontSize = "19";
        /// <summary>
        /// Size of the sample text. 
        /// </summary>
        public string FontSize
        {
            get { return mFontSize; }
            set
            {
                Regex regex = new Regex(@"[^0-9]");
                Match match = regex.Match(value);

                if (match.Success)
                    return;

                if (int.Parse(value) > 40)
                    value = "40";

                mFontSize = value;
            }
        }

        /// <summary>
        /// The size of the selected font sample text.
        /// </summary>
        public int SelectedFontSize { get; set; } = 19;

        /// <summary>
        /// private helper field.
        /// </summary>
        private string mSampleText;

        /// <summary>
        /// Sample text for the fonts.
        /// </summary>
        public string SampleText
        {
            get
            {
                SelectedSampleText = mSampleText;
                return mSampleText;
            }

            set
            {
                SelectedSampleText = mSampleText = value;
            }
        }

        /// <summary>
        /// The sample text of the selected font.
        /// </summary>
        public string SelectedSampleText { get; set; }

        /// <summary>
        /// The visibility of the listview.
        /// </summary>.
        public Visibility ListViewVisibility { get; set; } = Visibility.Visible;

        /// <summary>
        /// The visibility of the gridview.
        /// </summary>
        public Visibility GridViewVisibility { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// The visibility of the detailed font list.
        /// </summary>
        public Visibility DetailedFontVisibility { get; set; } = Visibility.Visible;

        /// <summary>
        /// The visibility of the compared font list.
        /// </summary>
        public Visibility ComparedFontVisibility { get; set; } = Visibility.Collapsed;
        #endregion

        #region Commands
        public ICommand LoadSampleTextCommand { get; private set; }

        public ICommand SliderCommand { get; private set; }

        public ICommand ViewCommand { get; private set; }

        public ICommand SelectedFontCommand { get; private set; }

        public ICommand AddToCompareCommand { get; private set; }

        public ICommand RemoveCompareCommand { get; private set; }

        public ICommand CompareFontsCommand { get; private set; }

        public ICommand SampleStyleCommand { get; private set; }

        public ICommand ChangeViewCommand { get; private set; }

        public ICommand DetailedViewCommand { get; private set; }

        #endregion

        #region Constructors
        public FontViewModel()
        {
           
            SystemFontsList = new ObservableCollection<FontModel>();
           
            // Initialize font list
            foreach (var font in System.Drawing.FontFamily.Families)
            {
                if (!string.IsNullOrEmpty(font.Name))
                {
                    string[] name = font.Name.Split(' ');

                    // Check if the font is already in the list, otherwise add as new.
                    if (SystemFontsList.Count != 0 && SystemFontsList.ElementAt(SystemFontsList.Count - 1).FontName.Equals(name[0]))
                    {
                        SystemFontsList.ElementAt(SystemFontsList.Count - 1).FontTypesList.Add(new DetailedFonts(font.Name));
                        SystemFontsList.ElementAt(SystemFontsList.Count - 1).AvailableTypes++;
                    }
                    else
                        SystemFontsList.Add(new FontModel { FontName = name[0], AvailableTypes = 1, FontFullName = font.Name, FontTypesList = new ObservableCollection<DetailedFonts>() { new DetailedFonts(font.Name) } });
                }
            }

            SampleText = "The Quick Brown Fox Jumps Over The Lazy Dog";
            ComparedFontsList = new ObservableCollection<DetailedFonts>();

            InitializeCommands();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializing commands.
        /// </summary>
        private void InitializeCommands()
        {
            LoadSampleTextCommand = new RelayParamCommand<Button>(LoadSampleText);
            AddToCompareCommand = new RelayParamCommand<string>(AddToCompare);
            RemoveCompareCommand = new RelayParamCommand<string>(RemoveCompareItem);
            SampleStyleCommand = new RelayParamCommand<string>(DisplayMode);
            ChangeViewCommand = new RelayParamCommand<string>(ChangeView);
            CompareFontsCommand = new RelayCommand(CompareFontsEvent);
        }

        /// <summary>
        /// Set the sample text.
        /// </summary>
        /// <param name="sender"></param>
        private void LoadSampleText(Button sender)
        {
            SampleText = sender.Content.ToString().Equals("Hu") ? "Árvíztűrő Tükörfúrógép" : "The Quick Brown Fox Jumps Over The Lazy Dog";
        }

        /// <summary>
        /// Add/Remove a font to/from the compare list.
        /// </summary>
        /// <param name="sender"></param>
        private void AddToCompare(string sender)
        {
   
               bool isDelete = false;

               var copyItem = SelectedFont.FontTypesList.Where(item => item.FontTypeName.Equals(sender)).Select
                (u => { u.IsSelected = u.IsSelected.Equals(true) ? false : true; isDelete = u.IsSelected; return u; }).ToList();

               if (isDelete == false)
                   ComparedFontsList.Remove(ComparedFontsList.Where(item => item.FontTypeName.Equals(sender)).Single());
               else
                   ComparedFontsList.Add(copyItem.ElementAt(0));
        }

        /// <summary>
        /// Remove a font from the compare view.
        /// </summary>
        /// <param name="sender"></param>
        private void RemoveCompareItem(string sender)
        {
            ComparedFontsList.Where(item => item.FontTypeName.Equals(sender)).Select
            (u => { u.IsSelected = false; return u; }).ToList();

            ComparedFontsList.Remove(ComparedFontsList.Where(item => item.FontTypeName.Equals(sender)).Single());
        }

        /// <summary>
        /// Set the font to upper or lower case.
        /// </summary>
        /// <param name="sender"></param>
        private void DisplayMode(string sender)
        {
            if (sender.Equals("aa"))
                SelectedSampleText = SelectedSampleText.ToLower();
            else
                SelectedSampleText  = SelectedSampleText.ToUpper();
        }

        /// <summary>
        /// Change the view from list to grid and back.
        /// </summary>
        /// <param name="sender"></param>
        private void ChangeView(string sender)
        {
            if (sender.Equals("GridButton"))
            {
                if (GridViewVisibility == Visibility.Collapsed)
                {
                    ListViewVisibility = Visibility.Collapsed;
                    GridViewVisibility = Visibility.Visible;
                }
            }

            else
            {
                if (ListViewVisibility == Visibility.Collapsed)
                {
                    GridViewVisibility = Visibility.Collapsed;
                    ListViewVisibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Change the view between the detailed and compared view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareFontsEvent()
        {
            if (DetailedFontVisibility == Visibility.Visible)
            {
                DetailedFontVisibility = Visibility.Hidden;
                ComparedFontVisibility = Visibility.Visible;
            }
            else
            {
                ComparedFontVisibility = Visibility.Hidden;
                DetailedFontVisibility = Visibility.Visible;
            }
        }
        #endregion
    }
}
