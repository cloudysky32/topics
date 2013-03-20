using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Comment
    {
        private int             _commentId;
        private int             _postId;
        private string          _userEmail;
        private string          _content;
        private string          _dateTime;

        public int CommentId
        {
            get { return this._commentId; }
            set { this._commentId = value; }
        }

        public int PostId
        {
            get { return this._postId; }
            set { this._postId = value; }
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

        public string DateTime
        {
            get { return this._dateTime; }
            set { this._dateTime = value; }
        }

        public Comment(int commentId, int postId, string userEmail, string content, string dateTime)
        {
            this._commentId = commentId;
            this._postId = postId;
            this._userEmail = userEmail;
            this._content = content;
            this._dateTime = dateTime;
        }
    }
}
