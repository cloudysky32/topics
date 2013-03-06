using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Issue
    {
        private int             _issueId;
        private int             _topicId;
        private string          _userEmail;
        private string          _content;
        private string          _imageUri;
        private int             _hot;
        private string          _datetime;

        public int IssueId
        {
            get { return this._issueId; }
            set { this._issueId = value; }
        }

        public int TopicId
        {
            get { return this._topicId; }
            set { this._topicId = value; }
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

        public int Hot
        {
            get { return this._hot; }
            set { this._hot = value; }
        }

        public string DateTime
        {
            get { return this._datetime; }
            set { this._datetime = value; }
        }

        public Issue(int issueId, int topicId, string userEmail, string content, string imageUri, int hot, string dateTime)
        {
            this.IssueId = issueId;
            this.TopicId = topicId;
            this.UserEmail = userEmail;
            this.Content = content;
            this.ImageUri = imageUri;
            this.Hot = hot;
            this.DateTime = dateTime;
        }
    }
}
