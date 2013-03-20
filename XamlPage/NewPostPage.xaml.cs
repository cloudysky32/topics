using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Topics.Util;
using Topics.Data;

namespace Topics.XamlPage
{
    public sealed partial class NewPostPage : UserControl
    {
        private string _communityId;
        public Popup PopupParent { get; set; }

        public delegate void SubmittedEventHandler(object sender, EventArgs e);
        public event SubmittedEventHandler SubmittedEvent;

        public NewPostPage(string communityId)
        {
            this.InitializeComponent();
            this._communityId = communityId;
        }

        private async void FindImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped || Windows.UI.ViewManagement.ApplicationView.TryUnsnap() == true)
            {
                Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

                // Filter to include a sample subset of file types.
                openPicker.FileTypeFilter.Clear();
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".jpg");

                // Open the file picker.
                Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();

                // file is null if user cancels the file picker.
                if (file != null)
                {
                    // Open a stream for the selected file.
                    Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    // Set the image source to the selected bitmap.
                    Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    displayImage.Source = bitmapImage;

                    ImageFile = file;
                }
                else
                {
                    ImageFile = null;
                }

                if (this.PopupParent != null)
                {
                    PopupParent.IsOpen = true;
                }
            }
        }

        public Windows.Storage.StorageFile ImageFile { get; set; }

        public async void SubmitPostButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog;
            HttpClientPostType httpClientPostType = new HttpClientPostType();

            this.submitPostButton.Visibility = Visibility.Collapsed;
            this.progressRing.IsActive = true;

            int result = -1;

            if (!this.postTextBox.Text.Trim().Equals("") && !(this.postTextBox.Text.Trim().Length < 10))
            {
                if (ImageFile == null)
                    result = await httpClientPostType.SubmitPost(this._communityId, User.Instance.Email, this.postTextBox.Text);
                else
                    result = await httpClientPostType.SubmitPost(this._communityId, User.Instance.Email, this.postTextBox.Text, ImageFile);
            }
            else
            {
                messageDialog = new MessageDialog("Please fill the form");
            }

            if (result != -1)
            {
                this.ImageFile = null;
                this.postTextBox.Text = string.Empty;

                SubmittedEvent(result, null);
            }

            this.submitPostButton.Visibility = Visibility.Visible;
            this.progressRing.IsActive = false;
        }
    }
}
