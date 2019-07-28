using System;
using System.Windows;
using System.Windows.Controls;

namespace MaterialDesign
{
    public partial class Icons : Page
    {
        public Icons()
        {
            InitializeComponent();
        }

        private void gif_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = new TimeSpan(0, 0, 1);
            gif.Play();
        }
    }
}
