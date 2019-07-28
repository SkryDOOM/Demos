using System.Collections.ObjectModel;

namespace MaterialDesign.Pages
{
    /// <summary>
    /// Used to create a color model, that contains the name of the color and every of it's hue.
    /// </summary>
    class ColorCodes : BaseViewModel
    {
        /// <summary>
        /// Name of the color (Json).
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// Contains the hues of the colors (Json).
        /// </summary>
        public ObservableCollection<string> ColorHexCode { get; set; }
    }
}