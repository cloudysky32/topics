using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Topic
    {
        private int _topicId;
        private int _categoryId;
        private string _topicName;
        private string _description;
        private string _imageUri;

        public int TopicId
        {
            get { return this._topicId; }
            set { this._topicId = value; }
        }

        public int CategoryId
        {
            get { return this._categoryId; }
            set { this._categoryId = value; }
        }

        public string TopicName
        {
            get { return this._topicName; }
            set { this._topicName = value; }
        }

        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        
        public string ImageUri
        {
            get { return this._imageUri; }
            set { this._imageUri = value; }
        }

        public Topic(int topicId, int categoryId, string topicName, string description, string imageUri)
        {
            this.TopicId = topicId;
            this.CategoryId = categoryId;
            this.TopicName = topicName;
            this.Description = description;
            this.ImageUri = imageUri;
        }
    }
}
