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
        private int             _issueId;
        private string          _userEmail;
        private string          _content;
        private string          _datetime;

        public int CommentId
        {
            get { return this._commentId; }
            set { this._commentId = value; }
        }

        public int IssueId
        {
            get { return this._issueId; }
            set { this._issueId = value; }
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
            get { return this._datetime; }
            set { this._datetime = value; }
        }
    }
}
