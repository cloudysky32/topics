using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Util.View;
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

        public DataCommon(int uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        public DataCommon() { }

        private int _uniqueId;
        public int UniqueId
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

        private int _itemTemplate = Topics.Util.View.ItemTemplates.DEFAULT;
        public int ItemTemplate
        {
            get { return this._itemTemplate; }
            set { this.SetProperty(ref this._itemTemplate, value); }
        }

        private bool _isTrue;
        public bool IsTrue
        {
            get { return this._isTrue; }
            set { this.SetProperty(ref this._isTrue, value); }
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
        public DataItem(int uniqueId, String title, String subtitle, String imagePath, String description, String content, DataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        public DataItem(int uniqueId, String title, String subtitle, String imagePath, String description, String content, DataGroup group, int itemTemplate)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
            this.ItemTemplate = itemTemplate;
        }

        public DataItem(int uniqueId, String title, String subtitle, String imagePath, String description, String content, DataGroup group, bool liked)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
            this.IsTrue = liked;
        }

        public DataItem(int uniqueId, String title, String subtitle, String imagePath, String description, String content, DataGroup group, dynamic originalSource)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
            this.OriginalSource = originalSource;
        }

        public DataItem(int uniqueId, String title, String subtitle, String imagePath, String description, String content, DataGroup group, bool liked, dynamic originalSource)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
            this.IsTrue = liked;
            this.OriginalSource = originalSource;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private dynamic _originalSource;
        public dynamic OriginalSource
        {
            get { return this._originalSource; }
            set { this.SetProperty(ref this._originalSource, value); }
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
        public DataGroup(int uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {

        }

        public DataGroup() { }

        private ObservableCollection<DataItem> _items = new ObservableCollection<DataItem>();
        public ObservableCollection<DataItem> Items
        {
            get { return this._items; }
            set { this.SetProperty(ref this._items, value); }
        }

        public void InitMainMenuDataGroup()
        {
            this.Items.Add(new DataItem(0, null, "Hot Topics", null, "Hot Topics", null, this));
            this.Items.Add(new DataItem(1, null, "Subscriptions", null, "Subscriptions", null, this));
            this.Items.Add(new DataItem(2, null, "Category", null, "Category", null, this));
        }

        public void InitCommunityMenuDataGroup()
        {
            this.Items.Add(new DataItem(0, null, "Hot Topics", null, "Hot Topics", null, this));
            this.Items.Add(new DataItem(1, null, "Weekly Topics", null, "Weekly Topics", null, this));
            this.Items.Add(new DataItem(2, null, "Posts", null, "Posts", null, this));
            this.Items.Add(new DataItem(3, null, "Votes", null, "Votes", null, this));
        }

        public void InitCategoryMenuDataGroup(List<Category> categoryList)
        {
            if (categoryList != null)
            {
                foreach (Category category in categoryList)
                {
                    if(!this.Items.Any(i => i.UniqueId == category.CategoryId))
                        this.Items.Add(new DataItem(category.CategoryId, category.CategoryName, category.CategoryName, category.Image, category.CategoryName, category.CategoryName, this));
                }
            }
        }

        public void StorePostsData(List<Post> postList)
        {
            if (postList != null)
            {
                foreach (Post post in postList)
                {
                    if (post.WeeklyTopics.Equals(string.Empty))
                    {
                        if (!this.Items.Any(i => i.UniqueId == post.PostId))
                            this.Items.Add(new DataItem(post.PostId, post.UserEmail, post.DateTime, post.ImageUri, post.LikeCount, post.Content, this, !post.Liked));

                    }
                    else
                    {
                        if (!this.Items.Any(i => i.UniqueId == post.PostId))
                            this.Items.Add(new DataItem(post.PostId, post.UserEmail, post.DateTime, post.ImageUri, post.LikeCount, post.Content, this, !post.Liked, post));
                    }

                }
            }
        }

        public void StoreSubscriptionsData(List<Community> subscriptionList)
        {
            if (subscriptionList != null)
            {
                foreach (Community community in subscriptionList)
                {
                    if(!this.Items.Any(i => i.UniqueId == community.CommunityId))
                        this.Items.Add(new DataItem(community.CommunityId, community.CommunityName, community.Master, community.Image, community.Description, community.UserCount.ToString(), this, true, community));
                }
            }
        }

        public void StoreCommentData(List<Comment> commentList)
        {
            if (commentList != null)
            {
                foreach (Comment comment in commentList)
                {
                    if (!this.Items.Any(i => i.UniqueId == comment.CommentId))
                    {
                        if (comment.UserEmail.Equals(User.Instance.Email))
                            this.Items.Add(new DataItem(comment.CommentId, comment.UserEmail, comment.DateTime, null, comment.Content, comment.Content, this, ItemTemplates.RIGHT_SIDE_LIST_VIEW_ITEM));
                        else
                            this.Items.Add(new DataItem(comment.CommentId, comment.UserEmail, comment.DateTime, null, comment.Content, comment.Content, this));
                    }
                }
            }
        }

        public bool StoreCategorizedCommunityData(List<Community> categorizedCommunityList)
        {
            if (categorizedCommunityList != null)
            {
                if (this.Items.Count != 0)
                    this.Items.Clear();

                foreach (Community community in categorizedCommunityList)
                {
                    if(User.Instance.Subscription.Items.Any(i => i.UniqueId == community.CommunityId))
                        this.Items.Add(new DataItem(community.CommunityId, community.CommunityName, community.Master, community.Image, community.Description, community.UserCount.ToString(), this, true, community));
                    else
                        this.Items.Add(new DataItem(community.CommunityId, community.CommunityName, community.Master, community.Image, community.Description, community.UserCount.ToString(), this, false, community));
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region Class : Data Model / has Data Groups(ItemGroups)
    public sealed class DataModel
    {
        private ObservableCollection<DataGroup> _itemGroups = new ObservableCollection<DataGroup>();
        public ObservableCollection<DataGroup> ItemGroups
        {
            get { return this._itemGroups; }
        }

        // Store Weekly Topics Data
        public void StoreWeeklyTopicsData(List<Post> postList)
        {
            if (postList != null)
            {
                foreach (Post post in postList)
                {
                    if (!this.ItemGroups.Any(i => i.UniqueId == post.Week))
                        this.ItemGroups.Add(new DataGroup(post.Week, post.Week.ToString(), post.WeeklyTopics, null, post.WeeklyTopics));
                }

                foreach (Post post in postList)
                {
                    DataGroup tmp = this.ItemGroups.Single(i => i.UniqueId == post.Week);
                    tmp.Items.Add(new DataItem(post.PostId, post.UserEmail, post.DateTime, post.ImageUri, post.LikeCount, post.Content, tmp, !post.Liked, post));
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
