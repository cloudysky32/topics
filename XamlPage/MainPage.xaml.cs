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
using Topics.Util;
using Topics.Data;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

namespace Topics.XamlPage
{
    public sealed partial class MainPage : Topics.Common.LayoutAwarePage
    {
        private static volatile DataSource _detailItemsDataSource = new DataSource();
        private static object _syncRoot = new Object();
        private static DataSource DetailItemsDataSource
        {
            get
            {
                if (_detailItemsDataSource == null)
                {
                    lock (_syncRoot)
                    {
                        if (_detailItemsDataSource == null)
                            _detailItemsDataSource = new DataSource();
                    }
                }
                return _detailItemsDataSource;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TextBlockChanger.Do(this.pageTitle);

            this.DefaultViewModel["UserPicture"] = new BitmapImage(new Uri(App.Current.Resources["UserPictureUri"].ToString(), UriKind.Absolute));
            this.DefaultViewModel["Groups"] = e.Parameter;
            this.DefaultViewModel["Categories"] = DetailItemsDataSource.ItemGroups;
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (pageState != null && pageState.ContainsKey("dataSource"))
            {
                this.DefaultViewModel["Groups"] = pageState["dataSource"];
            }
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            pageState["dataSource"] = this.DefaultViewModel["Groups"];
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }

        void Header_Click(object sender, RoutedEventArgs e)
        {
            var group = (sender as FrameworkElement).DataContext;

            DataGroup dataGroup = group as DataGroup;

            if (dataGroup.UniqueId.Equals("Category"))
            {
                //this.Frame.Navigate(typeof(GroupDetailPage), group);
            }
        }

        private async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataItem clickedItem = e.ClickedItem as DataItem;

            if (clickedItem.Group.UniqueId.Contains("Category") && _isDone)
            {
                DetailItemsDataSource.ItemGroups.Clear();
                _isDone = await SwapExpansion(clickedItem);
            }
            else if (clickedItem.Group.UniqueId.Contains("My Topics"))
            {
                this.Frame.Navigate(typeof(TopicPage), clickedItem);
            }
            else if (clickedItem.Group.UniqueId.Contains("Hot Issues"))
            {
                SwapExpansion_1(clickedItem);
            }
        }

        private void ItemDetailView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataGroup dataGroup = e.ClickedItem as DataGroup;
            this.Frame.Navigate(typeof(TopicsListPage), dataGroup);
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

            var chosenCommand = await menu.ShowForSelectionAsync(GetElementRect((FrameworkElement)sender));
        }

        private static DataItem _expandedItem = null;
        private static int _expandedItemIndex;
        private static bool _isDone = true;

        private async Task<bool> SwapExpansion(DataItem selectedDataItem)
        {
            _isDone = false;
            selectedDataItem.IsLoading = true;

            if (_expandedItem == null)
            {
                _expandedItem = selectedDataItem;
                _expandedItemIndex = selectedDataItem.Group.Items.IndexOf(selectedDataItem);

                selectedDataItem.Group.Items.Remove(selectedDataItem);
                _expandedItem.ItemTemplate = ItemTemplates.EXPANDED_GRID_VIEW_ITEM;

                selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                //selectedDataItem.ItemTemplate = ItemTemplates.EXPANDED_GRID_VIEW_ITEM;
                //itemGridView.Invalidate(selectedDataItem, this.TemplateSelector);

                HttpClientPostType httpClientPostType = new HttpClientPostType();
                List<Category> categoryList = await httpClientPostType.GetCategoryList(selectedDataItem.UniqueId);
                httpClientPostType.Dispose();

                if (categoryList != null)
                {
                    DetailItemsDataSource.StoreDetailCategoryDataSource(categoryList);
                    selectedDataItem.IsLoading = false;
                }
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
                    _expandedItem.ItemTemplate = ItemTemplates.EXPANDED_GRID_VIEW_ITEM;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                    HttpClientPostType httpClientPostType = new HttpClientPostType();
                    List<Category> categoryList = await httpClientPostType.GetCategoryList(selectedDataItem.UniqueId);
                    httpClientPostType.Dispose();

                    if (categoryList != null)
                    {
                        DetailItemsDataSource.StoreDetailCategoryDataSource(categoryList);
                        selectedDataItem.IsLoading = false;
                    }
                }
            }

            return true;
        }

        private static DataItem _expandedItem_1 = null;
        private static int _expandedItemIndex_1;
        private void SwapExpansion_1(DataItem selectedDataItem)
        {
            if (_expandedItem_1 == null)
            {
                _expandedItem_1 = selectedDataItem;
                _expandedItemIndex_1 = selectedDataItem.Group.Items.IndexOf(selectedDataItem);

                selectedDataItem.Group.Items.Remove(selectedDataItem);
                _expandedItem_1.ItemTemplate = ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM;

                selectedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem_1);

                //selectedDataItem.ItemTemplate = ItemTemplates.EXPANDED_GRID_VIEW_ITEM;
            }
            else
            {
                if (_expandedItem_1.Equals(selectedDataItem))
                {
                    selectedDataItem.Group.Items.Remove(selectedDataItem);
                    _expandedItem_1.ItemTemplate = ItemTemplates.DEFAULT;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex_1, _expandedItem_1);

                    //selectedDataItem.ItemTemplate = ItemTemplates.DEFAULT;

                    _expandedItem_1 = null;
                }
                else
                {
                    selectedDataItem.Group.Items.Remove(_expandedItem_1);
                    _expandedItem_1.ItemTemplate = ItemTemplates.DEFAULT;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex_1, _expandedItem_1);

                    _expandedItem_1 = selectedDataItem;
                    _expandedItemIndex_1 = selectedDataItem.Group.Items.IndexOf(selectedDataItem);

                    selectedDataItem.Group.Items.Remove(selectedDataItem);
                    _expandedItem_1.ItemTemplate = ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM;

                    selectedDataItem.Group.Items.Insert(_expandedItemIndex_1, _expandedItem_1);
                }
            }
        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private void GroupHeader_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockChanger.Do(sender);
        }
    }
}
