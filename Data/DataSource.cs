using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Topics.Data
{
    #region Abstract Class : Data Common
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class DataCommon : Topics.Common.BindableBase
    {
        private static Uri          _baseUri = new Uri("ms-appx:///");
        private static Uri          baseUri = new Uri("http://topics.azurewebsites.net/");

        public DataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(DataCommon.baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        private int _itemTemplate = Topics.Util.ItemTemplates.DEFAULT;
        public int ItemTemplate
        {
            get { return this._itemTemplate; }
            set { this.SetProperty(ref this._itemTemplate, value); }
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get { return this._isLoading; }
            set { this.SetProperty(ref this._isLoading, value); }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }
    #endregion

    #region Class : Data Item / implements DataCommon Class
    public class DataItem : DataCommon
    {
        public DataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, DataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private DataGroup _group;
        public DataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }
    #endregion

    #region Class : Data Group / implements DataCommon Class
    public class DataGroup : DataCommon
    {
        public DataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {

        }

        private ObservableCollection<DataItem> _items = new ObservableCollection<DataItem>();
        public ObservableCollection<DataItem> Items
        {
            get { return this._items; }
        }
    }
    #endregion

    #region Class : Data Source / has Data Groups(ItemGroups)
    public sealed class DataSource
    {
        private ObservableCollection<DataGroup> _itemGroups = new ObservableCollection<DataGroup>();
        public ObservableCollection<DataGroup> ItemGroups
        {
            get { return this._itemGroups; }
        }

        #region Method : Initialize Main Page Data Source
        public void InitMainPageDataSource(List<Issue> hotIssueList, List<Topic> myTopicList, List<Category> categoryList)
        {
            if (hotIssueList != null)
            {
                var hotIssueGroup = new DataGroup("Hot Issues", "Hot\nissues", "Hot Issues", "logo.png", "Hot Issues");

                foreach (Issue issue in hotIssueList)
                {
                    hotIssueGroup.Items.Add(new DataItem(issue.IssueId.ToString(), issue.Content, issue.UserEmail, issue.ImageUri, issue.Content, issue.Content, hotIssueGroup));
                }

                this.ItemGroups.Add(hotIssueGroup);
            }

            if (myTopicList != null)
            {
                var myTopicGroup = new DataGroup("My Topics", "My\nTopics", "My Topics", "logo.png", "My Topics");

                foreach (Topic subscription in myTopicList)
                {
                    if(subscription.ImageUri != null)
                        myTopicGroup.Items.Add(new DataItem(subscription.TopicId.ToString(), subscription.TopicName, subscription.TopicName, subscription.ImageUri, subscription.Description, subscription.Description, myTopicGroup));
                    else
                        myTopicGroup.Items.Add(new DataItem(subscription.TopicId.ToString(), subscription.TopicName, subscription.TopicName, "logo.png", subscription.Description, subscription.Description, myTopicGroup));
                }
                this.ItemGroups.Add(myTopicGroup);
            }

            if (categoryList != null)
            {
                var categoryGroup = new DataGroup("Category", "Bands\nof Topics", "Category", "logo.png", "Category");
                categoryGroup.ItemTemplate = Topics.Util.ItemTemplates.TALL_SIZE_ITEM;

                foreach (Category category in categoryList)
                {
                    categoryGroup.Items.Add(new DataItem(category.CategoryId.ToString(), category.CategoryName, category.CategoryName, category.ImageUri, category.CategoryName, category.CategoryName, categoryGroup));
                }
                this.ItemGroups.Add(categoryGroup);
            }
        }
        #endregion

        public void StoreDetailCategoryDataSource(List<Category> detailCategoryList)
        {
            if (detailCategoryList != null)
            {
                foreach (Category category in detailCategoryList)
                {
                    this.ItemGroups.Add(new DataGroup(category.CategoryId.ToString(), category.CategoryName, category.CategoryName, category.ImageUri, category.CategoryName));
                }
            }
        }

        public void StoreTopicDataSource(List<Topic> topicList)
        {
            if (topicList != null)
            {
                foreach (Topic topic in topicList)
                {
                    this.ItemGroups.Add(new DataGroup(topic.TopicId.ToString(), topic.TopicName, topic.TopicName, topic.ImageUri, topic.Description));
                }
            }
        }

        public void StoreIssueDataSource(List<Issue> issueList)
        {
            if (issueList != null)
            {
                foreach (Issue issue in issueList)
                {
                    this.ItemGroups.Add(new DataGroup(issue.IssueId.ToString(), issue.Content, issue.Content, issue.ImageUri, issue.Content));
                }
            }
        }
    }
    #endregion

#region Old DataSource
    /*
    class DataSource
    {
        private ItemCollection _Collection = new ItemCollection();

        public ItemCollection Collection
        {
            get
            {
                return this._Collection;
            }
        }

        public void StoreCategoryList(List<Category> categoryList)
        {
            Item item;
            Uri _baseUri = new Uri("ms-appx:///");
            Uri _base = new Uri("http://topics.azurewebsites.net/");

            foreach (Category category in categoryList)
            {
                item = new Item();
                item.Title = category.CategoryName;
                item.Category = "Category";

                //                item.SetImage(_baseUri, "Assets/topics_logo.png");
                item.SetImage(_base, "Logo.png");
                item.Subtitle = category.CategoryName;
                item.Link = "http://www.naver.com";
                item.Description = category.CategoryName;
                item.Collection = this.Collection;

                Collection.Add(item);
            }

//            for (int i = 0; i < category.Count; i++)
//            {
//                item = new Item();
//                item.Title = category[i].CategoryName;
//                item.Category = "Category";

////                item.SetImage(_baseUri, "Assets/topics_logo.png");
//                item.SetImage(_base, "Logo.png");
//                item.Subtitle = category[i].CategoryName;
//                item.Link = "http://www.naver.com";
//                item.Description = category[i].CategoryName;
//                item.Collection = this.Collection;

//                Collection.Add(item);
//            }
        }

        public void StoreFavoriteTopicList(List<Category> categoryList)
        {
            Item item;
            Uri _baseUri = new Uri("ms-appx:///");

            foreach (Category category in categoryList)
            {
                item = new Item();
                item.Title = category.CategoryName;
                item.Category = "Favorite Topics";

                item.SetImage(_baseUri, "Assets/topics_logo.png");
                item.Subtitle = category.CategoryName;
                item.Link = "http://www.naver.com";
                item.Description = category.CategoryName;
                item.Collection = this.Collection;

                Collection.Add(item);
            }

            //for (int i = 0; i < category.Count; i++)
            //{
            //    item = new Item();
            //    item.Title = category[i].CategoryName;
            //    item.Category = "Favorite Topics";

            //    item.SetImage(_baseUri, "Assets/topics_logo.png");
            //    item.Subtitle = category[i].CategoryName;
            //    item.Link = "http://www.naver.com";
            //    item.Description = category[i].CategoryName;
            //    item.Collection = this.Collection;

            //    Collection.Add(item);
            //}
        }

        public void StoreHotIssueList(List<Category> categoryList)
        {
            Item item;
            Uri _baseUri = new Uri("ms-appx:///");

            foreach (Category category in categoryList)
            {
                item = new Item();
                item.Title = category.CategoryName;
                item.Category = "Hot \nIssue";

                item.SetImage(_baseUri, "Assets/topics_logo.png");
                item.Subtitle = category.CategoryName;
                item.Link = "http://www.naver.com";
                item.Description = category.CategoryName;
                item.Collection = this.Collection;

                Collection.Add(item);
            }

            //for (int i = 0; i < category.Count; i++)
            //{
            //    item = new Item();
            //    item.Title = category[i].CategoryName;
            //    item.Category = "Hot \nIssue";

            //    item.SetImage(_baseUri, "Assets/topics_logo.png");
            //    item.Subtitle = category[i].CategoryName;
            //    item.Link = "http://www.naver.com";
            //    item.Description = category[i].CategoryName;

            //    Collection.Add(item);
            //}
        }

        #region Method : Sort by Category
        internal List<GroupInfoList<object>> GetGroupsByCategory()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

            var query = from item in Collection
//                        orderby ((Item)item).Category
                        group item by ((Item)item).Category into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            return groups;
        }
        #endregion

        #region Method : Sort By Group Name
        internal List<GroupInfoList<object>> GetGroupsByLetter()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

            var query = from item in Collection
                        orderby ((Item)item).Title
                        group item by ((Item)item).Title[0] into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            return groups;
        }
        #endregion
    }

    #region Class : Item in Group
    public class Item : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private string _Title = string.Empty;
        public string Title
        {
            get
            {
                return this._Title;
            }

            set
            {
                if (this._Title != value)
                {
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        private string _Subtitle = string.Empty;
        public string Subtitle
        {
            get
            {
                return this._Subtitle;
            }

            set
            {
                if (this._Subtitle != value)
                {
                    this._Subtitle = value;
                    this.OnPropertyChanged("Subtitle");
                }
            }
        }

        private ImageSource _Image = null;
        public ImageSource Image
        {
            get
            {
                return this._Image;
            }

            set
            {
                if (this._Image != value)
                {
                    this._Image = value;
                    this.OnPropertyChanged("Image");
                }
            }
        }

        public void SetImage(Uri baseUri, String path)
        {
            Image = new BitmapImage(new Uri(baseUri, path));
        }

        private string _Link = string.Empty;
        public string Link
        {
            get
            {
                return this._Link;
            }

            set
            {
                if (this._Link != value)
                {
                    this._Link = value;
                    this.OnPropertyChanged("Link");
                }
            }
        }

        private string _Category = string.Empty;
        public string Category
        {
            get
            {
                return this._Category;
            }

            set
            {
                if (this._Category != value)
                {
                    this._Category = value;
                    this.OnPropertyChanged("Category");
                }
            }
        }

        private string _Description = string.Empty;
        public string Description
        {
            get
            {
                return this._Description;
            }

            set
            {
                if (this._Description != value)
                {
                    this._Description = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }

        private string _Content = string.Empty;
        public string Content
        {
            get
            {
                return this._Content;
            }

            set
            {
                if (this._Content != value)
                {
                    this._Content = value;
                    this.OnPropertyChanged("Content");
                }
            }
        }

        private ItemCollection _Collection = new ItemCollection();
        public ItemCollection Collection
        {
            get
            {
                return this._Collection;
            }
            set
            {
                if (this._Collection != value)
                {
                    this._Collection = value;
                    this.OnPropertyChanged("Collection");
                }
            }
        }
    }
    #endregion

    public class GroupInfoList<T> : List<object>
    {
        public object Key { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<object>)base.GetEnumerator();
        }
    }

    public class ItemCollection : IEnumerable<Object>
    {
        private System.Collections.ObjectModel.ObservableCollection<Item> itemCollection = new System.Collections.ObjectModel.ObservableCollection<Item>();

        public IEnumerator<Object> GetEnumerator()
        {
            return itemCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Item item)
        {
            itemCollection.Add(item);
        }

        public int indexOf(Item item)
        {
            return itemCollection.IndexOf(item);
        }
    }
*/
#endregion
}
