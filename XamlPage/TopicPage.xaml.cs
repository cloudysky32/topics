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
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Topics.XamlPage
{
    public sealed partial class TopicPage : Topics.Common.LayoutAwarePage
    {
        private DataGroup _issueDataGroup;
        public DataGroup IssueDataGroup
        {
            get { return this._issueDataGroup; }
            set { this._issueDataGroup = value; }
        }

        private DataSource _twitsDataSource = new DataSource();
        public DataSource TwitsDataSource
        {
            get { return this._twitsDataSource; }
            set { this._twitsDataSource = value; }
        }

        private DataCommon _selectedTopic;
        public DataCommon SelectedTopic
        {
            get { return this._selectedTopic; }
            set { this._selectedTopic = value; }
        }

        private ObservableCollection<DataCommon> _topicDetail = new ObservableCollection<DataCommon>();
        public ObservableCollection<DataCommon> TopicDetail
        {
            get { return this._topicDetail; }
            set { this._topicDetail = value; }
        }

        private StorageFile _imageFile = null;
        public StorageFile ImageFile
        {
            get { return this._imageFile; }
            set { this._imageFile = value; }
        }

        public TopicPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (pageState != null && pageState.ContainsKey("ItemDetail"))
            {
                this.DefaultViewModel["ItemDetail"] = pageState["ItemDetail"];
                this.DefaultViewModel["Issues"] = pageState["Issues"];
            }
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            pageState["ItemDetail"] = this.DefaultViewModel["ItemDetail"];
            pageState["Issues"] = this.DefaultViewModel["Issues"];
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SelectedTopic = e.Parameter as DataCommon;

            pageTitle.Text = SelectedTopic.Title;
            TextBlockChanger.Do(pageTitle);

            SelectedTopic.ItemTemplate = ItemTemplates.DETAIL_VIEW_ITEM;
            TopicDetail.Add(SelectedTopic);
            await InitIssuesDataSource(SelectedTopic.UniqueId);

            this.DefaultViewModel["UserPicture"] = new BitmapImage(new Uri(App.Current.Resources["UserPictureUri"].ToString(), UriKind.Absolute));
            this.DefaultViewModel["Twits"] = TwitsDataSource.ItemGroups;
            this.DefaultViewModel["ItemDetail"] = TopicDetail;
        }

        private async Task InitIssuesDataSource(string topicId)
        {
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            List<Issue> issueList = await httpClientPostType.GetIssueList(topicId);

            if (issueList != null)
            {
                IssueDataGroup = new DataGroup(SelectedTopic.UniqueId.ToString(), SelectedTopic.Title, SelectedTopic.Title, SelectedTopic.Image.ToString(), SelectedTopic.Description);

                foreach (Issue issue in issueList)
                {
                    IssueDataGroup.Items.Add(new DataItem(issue.IssueId.ToString(), issue.Content, issue.UserEmail, issue.ImageUri, issue.Content, issue.Content, IssueDataGroup));
                }

                this.DefaultViewModel["Issues"] = IssueDataGroup.Items;

                this.loadingScreen.Visibility = Visibility.Collapsed;
            }
        }

        private async void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("Log Out", (command) =>
            {

            }));
            menu.Commands.Add(new UICommand("Subscriptions", (command) =>
            {

            }));

            var chosenCommand = await menu.ShowForSelectionAsync(MainPage.GetElementRect((FrameworkElement)sender));
        }

        private void ItemDetailVeiw_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataItem clickedItem = e.ClickedItem as DataItem;

            SwapExpansion(clickedItem);
        }

        private void PostIssueButton_Click(object sender, RoutedEventArgs e)
        {
            if (!postIssuePopup.IsOpen)
            {
                postIssuePopup.IsOpen = true;
                BottomAppBar.IsOpen = false;

                displayImage.Source = null;
                issueContentTextBox.Text = "";
            }            
        }

        private async void SubmitIssueButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog;
            HttpClientPostType httpClientPostType = new HttpClientPostType();

            this.loadingScreen.Visibility = Visibility.Visible;
            postIssuePopup.IsOpen = false;

            if (!issueContentTextBox.Text.Trim().Equals(""))
            {
                if (ImageFile == null)
                    await httpClientPostType.PostIssue(SelectedTopic.UniqueId, User.Instance.Email, issueContentTextBox.Text);
                else
                    await httpClientPostType.PostIssue(SelectedTopic.UniqueId, User.Instance.Email, issueContentTextBox.Text, ImageFile);

                await InitIssuesDataSource(SelectedTopic.UniqueId);
            }
            else
            {
                messageDialog = new MessageDialog("Please fill the form");
            }
            
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

                postIssuePopup.IsOpen = true;
            }
        }

        private static DataItem _expandedItem = null;
        private static int _expandedItemIndex;
        private void SwapExpansion(DataItem selectedDataItem)
        {
            if (_expandedItem == null)
            {
                _expandedItem = selectedDataItem;
                _expandedItemIndex = selectedDataItem.Group.Items.IndexOf(selectedDataItem);

                selectedDataItem.Group.Items.Remove(selectedDataItem);
                _expandedItem.ItemTemplate = ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM;

                selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                //selectedDataItem.ItemTemplate = ItemTemplates.EXPANDED_GRID_VIEW_ITEM;
            }
            else
            {
                if (_expandedItem.Equals(selectedDataItem))
                {
                    selectedDataItem.Group.Items.Remove(selectedDataItem);
                    _expandedItem.ItemTemplate = ItemTemplates.DEFAULT;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                    //selectedDataItem.ItemTemplate = ItemTemplates.DEFAULT;

                    _expandedItem = null;
                }
                else
                {
                    selectedDataItem.Group.Items.Remove(_expandedItem);
                    _expandedItem.ItemTemplate = ItemTemplates.DEFAULT;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                    _expandedItem = selectedDataItem;
                    _expandedItemIndex = selectedDataItem.Group.Items.IndexOf(selectedDataItem);

                    selectedDataItem.Group.Items.Remove(selectedDataItem);
                    _expandedItem.ItemTemplate = ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);
                }
            }
        }
    }
}
