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
    public sealed partial class SnappedHotTopicsPage : UserControl
    {
        private DataGroup _hotTopicsGridData = new DataGroup();
        private ItemExpander _itemExpander = new ItemExpander();
        private Popup _imagePopup = new Popup();

        public SnappedHotTopicsPage(string communityId)
        {
            this.InitializeComponent();
        }

        private async Task<bool> InitHotTopics(string communityId)
        {
            this.snappedHotTopicsGridView.ItemsSource = _hotTopicsGridData.Items;
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            _hotTopicsGridData.StorePostsData(await httpClientPostType.GetHotTopicList(communityId, User.Instance.Email));
            return false;
        }

        private async void SnappedHotTopicsPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = await InitHotTopics("0");
        }

        private void snappedHotTopicsGridView_Click(object sender, ItemClickEventArgs e)
        {
            DataItem clickedItem = e.ClickedItem as DataItem;

            _itemExpander.Swap(clickedItem, ItemTemplates.DEFAULT, ItemTemplates.EXPANDED_LIST_VIEW_ITEM);
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
    }
}
