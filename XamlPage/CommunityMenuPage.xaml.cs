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
using Windows.UI.Popups;

namespace Topics.XamlPage
{
    public sealed partial class CommunityMenuPage : UserControl
    {
        private Popup _popupPage = new Popup();
        private Popup _blankPopupPage = new Popup();
        private Popup _newPostPopup = new Popup();

        private SubscriptionsPage _subscriptionsPage = new SubscriptionsPage();
        private NewPostPage _newPostPage;
        private PostsPage _postsPage;
        private PostsPage _hotTopicsPage;
        private WeeklyTopicsPage _weeklyTopicsPage;
        //private PostsPage _weeklyTopicsPage;

        private DataGroup _communityMenuListData = new DataGroup();
        private DataItem _clickedCommunityItem;

        public CommunityMenuPage(DataItem clickedCommunityItem)
        {
            this.InitializeComponent();

            this._clickedCommunityItem = clickedCommunityItem;
            this.menuListView.ItemsSource = _communityMenuListData.Items;
        }

        private void CommunityMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.communityProfileGrid.DataContext = this._clickedCommunityItem;
            
            _newPostPage = new NewPostPage(this._clickedCommunityItem.UniqueId.ToString());
            _newPostPage.SubmittedEvent += _newPostPage_SubmittedEvent;

            App.PopupPage(this._blankPopupPage, App.popupPageGridWidth, new BlankPopupPage(), 2);
            this.subscribeButton.IsEnabled = !this.newPostButton.IsEnabled;

            this._communityMenuListData.InitCommunityMenuDataGroup();

            if (this.menuListView.ItemsSource != null)
                this.menuListView.SelectedIndex = 0;
        }

        private void _newPostPage_SubmittedEvent(object sender, EventArgs e)
        {
            int tmp = this.menuListView.SelectedIndex;
            this.menuListView.SelectedIndex = -1;
            this.menuListView.SelectedIndex = tmp;
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.menuListView.SelectedIndex != -1)
            {
                DataItem selectedItem = this.menuListView.SelectedItem as DataItem;
                this._popupPage.IsOpen = false;

                if (selectedItem.Description.Equals("Hot Topics"))
                {
                    if (this._hotTopicsPage == null)
                        this._hotTopicsPage = new PostsPage(this._clickedCommunityItem.UniqueId.ToString(), selectedItem.Description);

                    App.PopupPage(this._popupPage, App.popupPageGridWidth, this._hotTopicsPage, 2);
                }
                else if (selectedItem.Description.Equals("Weekly Topics"))
                {
                    if (this._weeklyTopicsPage == null)
                        this._weeklyTopicsPage = new WeeklyTopicsPage(this._clickedCommunityItem.UniqueId.ToString());
                        //this._weeklyTopicsPage = new PostsPage(this._clickedCommunityItem.UniqueId.ToString(), selectedItem.Description);

                    App.PopupPage(this._popupPage, App.popupPageGridWidth, this._weeklyTopicsPage, 2);
                }
                else if (selectedItem.Description.Equals("Posts"))
                {
                    if (this._postsPage == null)
                        this._postsPage = new PostsPage(this._clickedCommunityItem.UniqueId.ToString(), selectedItem.Description);

                    App.PopupPage(this._popupPage, App.popupPageGridWidth, this._postsPage, 2);
                }
                else
                {
                }
            }
        }

        private async void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("Sign Out", (command) =>
            {
                App.SignOut();
            }));

            var chosenCommand = await menu.ShowForSelectionAsync(App.GetElementRect((FrameworkElement)sender));
        }

        private void BackButton_GoBack(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
                this._popupPage.IsOpen = false;
                this._blankPopupPage.IsOpen = false;
            }
        }

        private void NewPostButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this._newPostPopup.IsOpen)
            {
                App.PopupPage(this._newPostPopup, 150, this._newPostPage, 1);
                this._newPostPage.PopupParent = this._newPostPopup;
            }
        }

        private async void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            if (await httpClientPostType.Subscribe(User.Instance.Email, this._clickedCommunityItem.UniqueId.ToString()))
                ((Button)sender).IsEnabled = false;
        }
    }
}
