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
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Topics.XamlPage
{
    public sealed partial class TopicsListPage : Topics.Common.LayoutAwarePage
    {
        private DataGroup _selectedCategory;
        public DataGroup SelectedCategory
        {
            get { return this._selectedCategory; }
            set { this._selectedCategory = value; }
        }

        private DataSource _topics = new DataSource();
        public DataSource Topics
        {
            get { return this._topics; }
            set { this._topics = value; }
        }

        private ObservableCollection<DataGroup> _topicDetail = new ObservableCollection<DataGroup>();
        public ObservableCollection<DataGroup> TopicDetail
        {
            get { return this._topicDetail; }
            set { this._topicDetail = value; }
        }

        private bool _isDone = true;
        public bool IsDone
        {
            get { return this._isDone; }
            set { this._isDone = value; }
        }

        public TopicsListPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SelectedCategory = e.Parameter as DataGroup;

            pageTitle.Text = SelectedCategory.Title;
            TextBlockChanger.Do(pageTitle);

            this.DefaultViewModel["UserPicture"] = new BitmapImage(new Uri(App.Current.Resources["UserPictureUri"].ToString(), UriKind.Absolute));
            this.DefaultViewModel["Groups"] = Topics.ItemGroups;
            this.DefaultViewModel["ItemDetail"] = TopicDetail;

            await InitTopicsDataSource(SelectedCategory.UniqueId);
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        public async void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGroup dataGroup = this.itemListView.SelectedItem as DataGroup;
            this.TopicDetail.Clear();
            this.TopicDetail.Add(dataGroup);

            if (!this.itemListView.IsItemClickEnabled & Topics.ItemGroups[Topics.ItemGroups.IndexOf(dataGroup)].Items.Count == 0)
            {
                this.itemListView.IsItemClickEnabled = true;

                HttpClientPostType httpClientPostType = new HttpClientPostType();
                //List<Category> categoryList = await httpClientPostType.GetCategoryList(dataGroup.UniqueId);
                List<Issue> issueList = await httpClientPostType.GetIssueList(dataGroup.UniqueId);

                if (issueList != null)
                {
                    foreach (Issue issue in issueList)
                    {
                        Topics.ItemGroups[Topics.ItemGroups.IndexOf(dataGroup)].Items.Add(new DataItem(issue.IssueId.ToString(), issue.Content, issue.UserEmail, issue.ImageUri, issue.Content, issue.Content, dataGroup));
                    }
                }

                this.itemListView.IsItemClickEnabled = false;
            }
        }

        private void ItemDetailVeiw_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataGroup clickedItem = e.ClickedItem as DataGroup;

            if (clickedItem.ItemTemplate == ItemTemplates.DETAIL_VIEW_ITEM)
                clickedItem.ItemTemplate = ItemTemplates.LIST_VIEW_ITEM;
            else
                clickedItem.ItemTemplate = ItemTemplates.DETAIL_VIEW_ITEM;

            DataGroup tmp = clickedItem;
            this.TopicDetail.Remove(clickedItem);
            this.TopicDetail.Add(tmp);
        }

        private void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataItem clickedItem = e.ClickedItem as DataItem;

            /*
            if (clickedItem.ItemTemplate == ItemTemplates.DEFAULT)
                clickedItem.ItemTemplate = ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM;
            else
                clickedItem.ItemTemplate = ItemTemplates.DEFAULT;

            DataItem tmp = clickedItem;
            int tmpIndex = clickedItem.Group.Items.IndexOf(clickedItem);
            
            clickedItem.Group.Items.RemoveAt(tmpIndex);
            clickedItem.Group.Items.Insert(tmpIndex, tmp);
            */
            SwapExpansion(clickedItem);
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

        // TODO : Modify to be topic data source, NOT category source
        private async Task InitTopicsDataSource(string categoryId)
        {
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            List<Topic> topicList = await httpClientPostType.GetTopicList(categoryId);

            if (topicList != null)
            {
                DataSource dataSource = new DataSource();
                dataSource.StoreTopicDataSource(topicList);
 
                Topics.ItemGroups.Clear();
                foreach (DataGroup dataGroup in dataSource.ItemGroups)
                {
                    dataGroup.ItemTemplate = ItemTemplates.DETAIL_VIEW_ITEM;
                    Topics.ItemGroups.Add(dataGroup);
                }
            }

            this.loadingScreen.Visibility = Visibility.Collapsed;
            //this.itemListView.Visibility = Visibility.Visible;
            //this.itemDetail.Visibility = Visibility.Visible;
        }

        private void CreateTopicButton_Click(object sender, RoutedEventArgs e)
        {
            if (!createTopicPopup.IsOpen)
            {
                createTopicPopup.IsOpen = true;
                BottomAppBar.IsOpen = false;

                topicNameTextBox.Text = "";
                descriptionTextBox.Text = "";
            }
        }

        private async void SubmitTopicButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog;
            HttpClientPostType httpClientPostType = new HttpClientPostType();

            loadingScreen.Visibility = Visibility.Visible;
            createTopicPopup.IsOpen = false;

            if (!topicNameTextBox.Text.Trim().Equals("") && !descriptionTextBox.Text.Trim().Equals(""))
            {
                if (await httpClientPostType.CreateTopic(User.Instance.Email, topicNameTextBox.Text, descriptionTextBox.Text, SelectedCategory.UniqueId))
                    messageDialog = new MessageDialog("Success to create Topic!");
                else
                    messageDialog = new MessageDialog("Fail to create Topic");
            }
            else
            {
                messageDialog = new MessageDialog("Please fill the form");
            }

            await InitTopicsDataSource(SelectedCategory.UniqueId);
            await messageDialog.ShowAsync();
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
