using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Topics.XamlPage
{
    public sealed partial class ImagePage : UserControl
    {
        private Windows.UI.Xaml.Controls.Image _image;

        public ImagePage(Windows.UI.Xaml.Controls.Image imageSource)
        {
            this.InitializeComponent();
            this._image = imageSource;
        }

        private void BackButton_GoBack(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }
        }

        private void ImagePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.image.Source = this._image.Source;
        }
    }
}
