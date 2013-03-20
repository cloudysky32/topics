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
using Topics.Data;
using Topics.Util;
using Topics.Util.View;
using System.Threading.Tasks;

namespace Topics.XamlPage
{
    public sealed partial class PostsPage : UserControl
    {
        private Popup _commentPopup = new Popup();
        private Popup _imagePopup = new Popup();
        private DataGroup _postsGridData = new DataGroup();
        private ItemExpander _itemExpander = new ItemExpander();
        private string _communityId;
        private string _header;

        public PostsPage(string communityId)
        {
            this.InitializeComponent();
            this.postsGridView.ItemsSource = _postsGridData.Items;
            this._communityId = communityId;
        }

        public PostsPage(string communityId, string header)
        {
            this.InitializeComponent();
            this.postsGridView.ItemsSource = _postsGridData.Items;
            this._communityId = communityId;
            this._header = header;
        }

        private async Task<bool> InitPosts(string communityId)
        {
            this.progressRing.IsActive = true;
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            this.header.Text = this._header;

            if(this._header.Equals("Hot Topics"))
                _postsGridData.StorePostsData(await httpClientPostType.GetHotTopicList(communityId, User.Instance.Email));
            else if(this._header.Equals("Posts"))
                _postsGridData.StorePostsData(await httpClientPostType.GetPostList(communityId, User.Instance.Email));
            else if(this._header.Equals("Weekly Topics"))
                _postsGridData.StorePostsData(await httpClientPostType.GetWeeklyTopicList(communityId, User.Instance.Email));

            return false;
        }

        private async void PostsPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = await InitPosts(this._communityId);
        }

        private void PostsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this._commentPopup.IsOpen = false;
        }

        private void PostsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataItem clickedItem = e.ClickedItem as DataItem;

            if (clickedItem != null)
            {
                if (this._commentPopup.IsOpen)
                    this._commentPopup.IsOpen = false;

                if (clickedItem.ItemTemplate == ItemTemplates.DEFAULT)
                    App.PopupPage(_commentPopup, this.commentGrid.ActualWidth, new CommentPage(clickedItem.UniqueId.ToString()), 2);

                _itemExpander.Swap(clickedItem, ItemTemplates.DEFAULT, ItemTemplates.EXPANDED_LIST_VIEW_ITEM);
            }
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            Button likeButton = sender as Button;
            this.progressRing.IsActive = true;

            HttpClientPostType httpClientPostType = new HttpClientPostType();
            if (this._itemExpander.ExpandedItem != null)
            {
                if (await httpClientPostType.LikePost(this._itemExpander.ExpandedItem.UniqueId.ToString(), User.Instance.Email, "1"))
                {
                    likeButton.IsEnabled = false;
                    _itemExpander.ExpandedItem.Description = (Int16.Parse(_itemExpander.ExpandedItem.Description) + 1).ToString();
                }
                else
                {
                    likeButton.IsEnabled = false;
                    await new Windows.UI.Popups.MessageDialog("You've already like it").ShowAsync();
                }
            }
            this.progressRing.IsActive = false;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            Button imageButton = sender as Button;

            App.PopupPage(this._imagePopup, 1366, new ImagePage(imageButton.Content as Windows.UI.Xaml.Controls.Image), 0);
        }
    }
}
