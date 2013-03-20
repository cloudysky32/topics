using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Category
    {
        //community_id, community_name, category_id, description, image, community_datetime, era, era_datetime, community_master, master_datetime, user_count, post_count
        private int             _categoryId;
        private string          _categoryName;
        private int             _parentId;
        private string          _image;

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

        public string Image
        {
            get { return this._image; }
            set { this._image = value; }
        }

        public Category(int CategoryId, string CategoryName, int ParentId, string Image)
        {
            this.CategoryId = CategoryId;
            this.CategoryName = CategoryName;
            this.ParentId = ParentId;
            this.Image = Image;
        }
    }
}
