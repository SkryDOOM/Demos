
namespace MaterialDesign.Pages
{
    class DetailedFonts : BaseViewModel
    {
        /// <summary>
        /// Fully qualified name of a font type.
        /// </summary>
        public string FontTypeName { get; set; }

        /// <summary>
        /// Indicates if a font was selected to compare.
        /// </summary>
        public bool IsSelected { get; set; }

        public DetailedFonts() { }

        public DetailedFonts(string fontTypeName)
        {
           FontTypeName = fontTypeName;
           IsSelected = false;
        }

    }
}
