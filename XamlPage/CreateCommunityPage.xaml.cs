using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Topics.Data;
using Topics.Util;
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

namespace Topics.XamlPage
{
    public sealed partial class CreateCommunityPage : UserControl
    {
        public Windows.Storage.StorageFile ImageFile { get; set; }
        private string _subCategoryId = string.Empty;

        public CreateCommunityPage(string subCategoryId)
        {
            this.InitializeComponent();
            this._subCategoryId = subCategoryId;
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

                if (this.Parent.GetType() == typeof(Popup))
                {
                    ((Popup)this.Parent).IsOpen = true;
                }
            }
        }

        private async void SubmitPostButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this._subCategoryId.Equals(string.Empty))
            {
                MessageDialog messageDialog = null;
                HttpClientPostType httpClientPostType = new HttpClientPostType();

                this.progressRing.IsActive = true;
                this.submitPostButton.IsEnabled = false;

                bool imageStatus = false;
                bool nameStatus = false;
                bool descriptionStatus = false;

                if (ImageFile != null)
                    imageStatus = true;
                else
                    imageStatus = false;

                if (!communityNameTextBox.Text.Trim().Equals("") && communityNameTextBox.Text.Trim().Length > 1)
                    nameStatus = true;
                else
                    nameStatus = false;

                if (!communityDescriptionTextBox.Text.Trim().Equals("") && communityDescriptionTextBox.Text.Trim().Length > 10)
                    descriptionStatus = true;
                else
                    descriptionStatus = false;

                if (imageStatus && nameStatus && descriptionStatus)
                {
                    if (await httpClientPostType.CreateCommunity(User.Instance.Email, communityNameTextBox.Text, communityDescriptionTextBox.Text, _subCategoryId, ImageFile))
                    {
                        messageDialog = new MessageDialog("Successfully Create Community!");

                        if (this.Parent.GetType() == typeof(Popup))
                        {
                            ((Popup)this.Parent).IsOpen = false;
                        }
                    }
                    else
                    {
                        messageDialog = new MessageDialog("Sorry, failed to create community. Please try later.");
                    }
                }
                else
                {
                    messageDialog = new MessageDialog("Please fill the form.");
                }

                this.submitPostButton.IsEnabled = true;
                this.progressRing.IsActive = false;

                if(messageDialog != null)
                    await messageDialog.ShowAsync();
            }
        }

        private void BackButton_GoBack(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }
        }
    }
}
