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
using System.Threading.Tasks;

namespace Topics.XamlPage
{
    public sealed partial class SubscriptionsPage : UserControl
    {
        private DataGroup           _subscriptionsListData = new DataGroup();
        private Popup               _popupPage = new Popup();

        public SubscriptionsPage()
        {
            this.InitializeComponent();
        }

        private async void SubscriptionPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = await InitSubscriptions();
        }

        private async Task<bool> InitSubscriptions()
        {
            subscriptionsGridView.ItemsSource = this._subscriptionsListData.Items;

            HttpClientPostType httpClientPostType = new HttpClientPostType();
            _subscriptionsListData.StoreSubscriptionsData(await httpClientPostType.GetSubscriptionList(User.Instance.Email));

            return false;
        }

        public void SubscriptionGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.PopupPage(this._popupPage, App.menuGridWidth, new CommunityMenuPage(e.ClickedItem as DataItem), 0);  
        }
    }
}
