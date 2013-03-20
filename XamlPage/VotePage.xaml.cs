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
    public sealed partial class VotePage : UserControl
    {
        private Popup _commentPopup = new Popup();
        private Popup _voteConditionPopup = new Popup();
        private DataGroup _voteGridData = new DataGroup();
        private ItemExpander _itemExpander = new ItemExpander();
        private string _communityId;

        public VotePage(string communityId)
        {
            this.InitializeComponent();
            this.VoteGridView.ItemsSource = _voteGridData.Items;
            this._communityId = communityId;
        }

        private bool InitVote(string communityId)
        {
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            //_voteGridData.StoreVoteData(await httpClientPostType.GetVoteList(communityId));

            return false;
        }

        private void VotePage_Loaded(object sender, RoutedEventArgs e)
        {
            //this.progressRing.IsActive = await InitVote(this._communityId);
        }

        private void VotePage_Unloaded(object sender, RoutedEventArgs e)
        {
            this._commentPopup.IsOpen = false;
        }

        private void VoteGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataItem clickedItem = e.ClickedItem as DataItem;

            if (clickedItem != null)
            {
                if (this._commentPopup.IsOpen)
                    this._commentPopup.IsOpen = false;

                if (clickedItem.ItemTemplate == ItemTemplates.DEFAULT)
                    App.PopupPage(_commentPopup, this.commentGrid.ActualWidth, new Privacy(), 2);

                _itemExpander.Swap(clickedItem, ItemTemplates.DEFAULT, ItemTemplates.EXPANDED_LIST_VIEW_ITEM);
            }
        }
    }
}
