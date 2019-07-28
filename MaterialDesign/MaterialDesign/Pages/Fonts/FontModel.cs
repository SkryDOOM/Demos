using System.Collections.ObjectModel;

namespace MaterialDesign.Pages
{
    class FontModel : BaseViewModel
    {
        /// <summary>
        /// Short name of a font.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Fully qualified name of a font to set the fontfamily.
        /// </summary>
        public string FontFullName { get; set; }

        /// <summary>
        /// Collection of a font type.
        /// </summary>
        public ObservableCollection<DetailedFonts> FontTypesList { get; set; }

        /// <summary>
        /// Displays how many variant of a font are available.
        /// </summary>
        public int AvailableTypes { get; set; }
    }
}