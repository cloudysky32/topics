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
    public sealed partial class CategoryMenuPage : UserControl
    {
        private Popup _popupPage = new Popup();
        private Popup _createCommunityPopup = new Popup();
        private SubCategoryPage _subCategoryPage;
        private DataGroup _categoryMenuListData = new DataGroup();

        public CategoryMenuPage()
        {
            this.InitializeComponent();
            InitCategoryMenu();
        }

        private async void InitCategoryMenu()
        {
            this.menuListView.ItemsSource = _categoryMenuListData.Items;
            this._categoryMenuListData.InitCategoryMenuDataGroup(await new HttpClientPostType().GetCategoryList("0"));
            this.createCommunityButton.IsEnabled = false;
        }

        private void CategoryMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.menuListView.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                InitCategoryMenu();
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        private void BackButton_GoBack(object sender, RoutedEventArgs e)
        {
            this.menuListView.SelectedIndex = -1;
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
                this._popupPage.IsOpen = false;
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

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.menuListView.SelectedIndex != -1)
            {
                DataItem selectedItem = this.menuListView.SelectedItem as DataItem;
                this._popupPage.IsOpen = false;
                this.createCommunityButton.IsEnabled = false;

                this._subCategoryPage = new SubCategoryPage(selectedItem);
                this._subCategoryPage.SelectionChangedEvent += _subCategoryPage_SelectionChangedEvent;

                App.PopupPage(this._popupPage, App.popupPageGridWidth, this._subCategoryPage, 2);
            }
        }

        private void _subCategoryPage_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            this.createCommunityButton.IsEnabled = listView.SelectedIndex >= 0 ? true : false;
        }

        private void CreateCommunityButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._subCategoryPage._selectedSubCategoryId != -1)
                App.PopupPage(this._createCommunityPopup, 1366, new CreateCommunityPage(this._subCategoryPage._selectedSubCategoryId.ToString()), 0);
        }
    }
}
