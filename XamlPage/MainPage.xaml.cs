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
    public sealed partial class MainPage : Topics.Common.LayoutAwarePage
    {
        private Popup               _popupPage = new Popup();

        private SubscriptionsPage   _subscriptionsPage = new SubscriptionsPage();
        private CategoryMenuPage    _categoryMenuPage = new CategoryMenuPage();
        private PostsPage           _hotTopcisPage;

        private DataGroup           _mainMenuListData = new DataGroup();

        public MainPage()
        {
            this.InitializeComponent();
            InitMainMenu();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.menuGridWidth = this.menuGrid.ActualWidth;
            App.popupPageGridWidth = this.popupPageGrid.ActualWidth;
            App.commentGridWidth = this.commentGrid.ActualWidth;

            this.menuListView.SelectedIndex = 0;
        }

        private void InitMainMenu()
        {
            this.DefaultViewModel["Menu"] = _mainMenuListData.Items;
            this._mainMenuListData.InitMainMenuDataGroup();
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataItem selectedItem = this.menuListView.SelectedItem as DataItem;
            this._popupPage.IsOpen = false;

            if (selectedItem.Description.Equals("Hot Topics"))
            {
                if (this._hotTopcisPage == null)
                    this._hotTopcisPage = new PostsPage("0", selectedItem.Description);

                App.PopupPage(this._popupPage, App.popupPageGridWidth, this._hotTopcisPage, 2);
            }
            else if (selectedItem.Description.Equals("Subscriptions"))
            {
                App.PopupPage(this._popupPage, App.popupPageGridWidth, this._subscriptionsPage, 2);
            }
            else if (selectedItem.Description.Equals("Category"))
            {
                App.PopupPage(this._popupPage, App.menuGridWidth, this._categoryMenuPage, 0); 
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
    }
}
