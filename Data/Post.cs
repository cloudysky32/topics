using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Post
    {
        private int             _postId;
        private int             _communityId;
        private string          _userEmail;
        private string          _content;
        private string          _imageUri;
        private string          _likeCount;
        private string          _datetime;
        private bool            _liked;
        private string          _weeklyTopics = string.Empty;
        private int             _week = -1;

        public int PostId
        {
            get { return this._postId; }
            set { this._postId = value; }
        }

        public int CommunityId
        {
            get { return this._communityId; }
            set { this._communityId = value; }
        }

        public string UserEmail
        {
            get { return this._userEmail; }
            set { this._userEmail = value; }
        }

        public string Content
        {
            get { return this._content; }
            set { this._content = value; }
        }

        public string ImageUri
        {
            get { return this._imageUri; }
            set { this._imageUri = value; }
        }

        public string LikeCount
        {
            get { return this._likeCount; }
            set { this._likeCount = value; }
        }

        public bool Liked
        {
            get { return this._liked; }
            set { this._liked = value; }
        }

        public string DateTime
        {
            get { return this._datetime; }
            set { this._datetime = value; }
        }

        public string WeeklyTopics
        {
            get { return this._weeklyTopics; }
            set { this._weeklyTopics = value; }
        }

        public int Week
        {
            get { return this._week; }
            set { this._week = value; }
        }

        public Post(int postId,
                    int communityId,
                    string userEmail,
                    string content,
                    string imageUri,
                    string likeCount,
                    bool liked,
                    string dateTime)
        {
            this.PostId = postId;
            this.CommunityId = CommunityId;
            this.UserEmail = userEmail;
            this.Content = content;
            this.ImageUri = imageUri;
            this.LikeCount = likeCount;
            this.Liked = liked;
            this.DateTime = dateTime;
        }

        public Post(int postId,
                    int communityId,
                    string userEmail,
                    string content, 
                    string imageUri,
                    string likeCount,
                    bool liked,
                    string dateTime,
                    string weeklyTopics,
                    int week)
        {
            this.PostId = postId;
            this.CommunityId = CommunityId;
            this.UserEmail = userEmail;
            this.Content = content;
            this.ImageUri = imageUri;
            this.LikeCount = likeCount;
            this.Liked = liked;
            this.DateTime = dateTime;
            this.WeeklyTopics = weeklyTopics;
            this.Week = week;
        }
    }
}
