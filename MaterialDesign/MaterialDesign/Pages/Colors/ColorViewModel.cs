using MaterialDesign.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace MaterialDesign.Pages
{
    class ColorViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Contains the code of the colors.
        /// </summary>
        public ObservableCollection<ColorCodes> ColorsList { get; private set; }

        /// <summary>
        /// Contains the unique color codes.
        /// </summary>
        public ObservableCollection<UniqueColors> UniqueColorsList { get; private set; }

        private string mSelectedColorCode = "#ffffff";
        /// <summary>
        /// Contains the hexcode of the selected color.
        /// </summary>
        public string SelectedColorcode {
            get { return mSelectedColorCode; }
            set
            {
                Regex regex = new Regex("#FF([0-9|A-F|a-f]{6})");
                Match match = regex.Match(value);

                mSelectedColorCode = match.Success ? "#" + match.Groups[1].Value : mSelectedColorCode = value;
            }
        }

        private string mFlat = "";
        /// <summary>
        /// A helper property to highlight the selected flat color.
        /// </summary>
        public string FlatColorValue
        {
            get { return mFlat; }
            set
            {
                SelectedColorcode = value;
                mFlat = value;
            }
        }

        /// <summary>
        /// Contains the selected unique color code.
        /// </summary>
        public string SelectedUniqueColor { get; set; } = "#ffffff";

        /// <summary>
        /// Indicates if a unique color was selected to edit.
        /// </summary>
        private bool mIsEdit;

        /// <summary>
        /// Path to the color codes.
        /// </summary>
        private static readonly string JsonColors = "Pages/Colors/ColorCodes.json";

        /// <summary>
        /// Path to the unique color codes.
        /// </summary>
        private static readonly string JsonUnique = "Pages/Colors/UniqueColors.json";
        #endregion

        #region Commands
        /// <summary>
        /// Copy the selected color to clipboard.
        /// </summary>
        public ICommand CopyColorCommand { get; set; }

        /// <summary>
        /// Add the selected color to the unique colors.
        /// </summary>
        public ICommand SaveColorCommand { get; set; }

        /// <summary>
        /// Remove the selected unique color.
        /// </summary>
        public ICommand RemoveColorCommand { get; set; }

        /// <summary>
        /// Jump to the edit panel.
        /// </summary>
        public ICommand EditColorCommand { get; set; }

        /// <summary>
        /// Copy the unique color to clipboad.
        /// </summary>
        public ICommand CopyUniqueCommand { get; set; }

        /// <summary>
        /// Gets the hexcode of the selected color.
        /// </summary>
        public ICommand SelectColorCommand { get; set; }

        /// <summary>
        /// Get the selected unique color.
        /// </summary>
        public ICommand SelectedUniqueCommand { get; set; }
        #endregion

        #region Constructors
        public ColorViewModel()
        {
            ColorsList = JsonConvert.DeserializeObject<ObservableCollection<ColorCodes>>((JArray.Parse(File.ReadAllText(JsonColors))).ToString(Formatting.Indented));
            UniqueColorsList = JsonConvert.DeserializeObject<ObservableCollection<UniqueColors>>((JArray.Parse(File.ReadAllText(JsonUnique))).ToString(Formatting.Indented));
            InitializeCommands();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the commands.
        /// </summary>
        private void InitializeCommands()
        {
            SelectColorCommand = new RelayParamCommand<string>((string sender) => { SelectedColorcode = sender; });
            CopyColorCommand = new RelayCommand(() => { if (!string.IsNullOrWhiteSpace(mSelectedColorCode)) Clipboard.SetText(mSelectedColorCode); });
            CopyUniqueCommand = new RelayCommand(() => { if (!string.IsNullOrEmpty(SelectedUniqueColor)) Clipboard.SetText(SelectedUniqueColor); });
            SelectedUniqueCommand = new RelayParamCommand<string>((string sender) => { SelectedUniqueColor = sender; });            
            SaveColorCommand = new RelayCommand(SaveColor);
            RemoveColorCommand = new RelayCommand(RemoveColor);
            EditColorCommand = new RelayCommand(EditColor);
        }

        /// <summary>
        /// Add color to the unique list.
        /// </summary>
        private void SaveColor()
        {
            // Don't add the new color if color already exists.
            var isExist = UniqueColorsList.FirstOrDefault(x => x.UniqueColor.Equals(SelectedColorcode));
            if (isExist != null)
                return;

            if (!mIsEdit)
            {
                UniqueColorsList.Add(new UniqueColors { UniqueColor = mSelectedColorCode });
            }
            else
            {
                mIsEdit = false;

                // Replacing the old value with the new.
                var item = UniqueColorsList.FirstOrDefault(i => i.UniqueColor.Equals(SelectedUniqueColor));
                if (item == null) return;
                item.UniqueColor = SelectedColorcode;
                SelectedUniqueColor = SelectedColorcode;
            }

            File.WriteAllText(JsonUnique, JsonConvert.SerializeObject(UniqueColorsList, Formatting.Indented));
        }

        /// <summary>
        /// Remove the selected unique color.
        /// </summary>
        private void RemoveColor()
        {
            if (string.IsNullOrEmpty(SelectedUniqueColor))
                return;

            var itemRemove = UniqueColorsList.FirstOrDefault(item => item.UniqueColor.Equals(SelectedUniqueColor));

            if (itemRemove == null) return;

            UniqueColorsList.Remove(itemRemove);
            File.WriteAllText(JsonUnique, JsonConvert.SerializeObject(UniqueColorsList, Formatting.Indented));

            SelectedUniqueColor = "#ffffff";
        }

        /// <summary>
        /// Indicates that a unique color should be edited.
        /// </summary>
        private void EditColor()
        {
            if (string.IsNullOrEmpty(SelectedUniqueColor))
                return;

            SelectedColorcode = SelectedUniqueColor;
            mIsEdit = true;            
        }
        #endregion
    }
}