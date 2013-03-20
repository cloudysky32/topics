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
    public sealed partial class SubCategoryPage : UserControl
    {
        public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
        public event SelectionChangedEventHandler SelectionChangedEvent;

        private Popup           _popupPage = new Popup();
        private ItemExpander    _itemExpander = new ItemExpander();
        private DataGroup       _subCategoryListData = new DataGroup();
        private DataGroup       _categorizedCommunityListData = new DataGroup();
        private DataItem        _selectedCategory;
        public int              _selectedSubCategoryId = -1;

        public SubCategoryPage(DataItem selectedCategory)
        {
            this.InitializeComponent();
            this._selectedCategory = selectedCategory;
            subCategoryListViewHeader.Text = this._selectedCategory.Title;
            subCategoryListView.ItemsSource = this._subCategoryListData.Items;
            categorizedCommunityGridView.ItemsSource = this._categorizedCommunityListData.Items;
        }

        private async Task<bool> InitSubCategory()
        {
            this._subCategoryListData.InitCategoryMenuDataGroup(await new HttpClientPostType().GetCategoryList(_selectedCategory.UniqueId.ToString()));
            return false;
        }

        private async void SubCategoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = await InitSubCategory();
        }

        private async void SubCategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.progressRing.IsActive = true;
            this._categorizedCommunityListData.Items.Clear();

            DataItem selectedItem = this.subCategoryListView.SelectedItem as DataItem;
            this._selectedSubCategoryId = selectedItem.UniqueId;

            HttpClientPostType httpClientPostType = new HttpClientPostType();
            this.progressRing.IsActive = !this._categorizedCommunityListData.StoreCategorizedCommunityData(await httpClientPostType.GetCommunityList(selectedItem.UniqueId.ToString()));

            SelectionChangedEvent(sender, null);
        }

        private void CategorizedCommunityGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.PopupPage(this._popupPage, App.menuGridWidth, new CommunityMenuPage(e.ClickedItem as DataItem), 0);
        }
    }
}
