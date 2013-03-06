using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Category
    {
        private int             _categoryId;
        private string          _categoryName;
        private int             _parentId;
        private string          _imageUri;

        public int CategoryId
        {
            get { return this._categoryId; }
            set { this._categoryId = value; }
        }

        public string CategoryName
        {
            get { return this._categoryName; }
            set { this._categoryName = value; }
        }

        public int ParentId
        {
            get { return this._parentId; }
            set { this._parentId = value; }
        }

        public string ImageUri
        {
            get { return this._imageUri; }
            set { this._imageUri = value; }
        }

        public Category(int CategoryId, string CategoryName, int ParentId, string ImageUri)
        {
            this.CategoryId = CategoryId;
            this.CategoryName = CategoryName;
            this.ParentId = ParentId;
            this.ImageUri = ImageUri;
        }
    }
}
