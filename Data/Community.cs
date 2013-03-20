using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    public class Community
    {
        private int _communityId;
        private int _categoryId;
        private string _communityName;
        private string _description;
        private string _image;
        private string _communityDateTime;

        private string _era;
        private string _eraDateTime;
        private string _master;
        private string _masterDateTime;
        private int _userCount;
        private int _postCount;

        public int CommunityId
        {
            get { return this._communityId; }
            set { this._communityId = value; }
        }

        public int CategoryId
        {
            get { return this._categoryId; }
            set { this._categoryId = value; }
        }

        public string CommunityName
        {
            get { return this._communityName; }
            set { this._communityName = value; }
        }

        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        
        public string Image
        {
            get { return this._image; }
            set { this._image = value; }
        }

        public string CommunityDateTime
        {
            get { return this._communityDateTime; }
            set { this._communityDateTime = value; }
        }

        public string Era
        {
            get { return this._era; }
            set { this._era = value; }
        }

        public string EraDateTime
        {
            get { return this._eraDateTime; }
            set { this._eraDateTime = value; }
        }

        public string Master
        {
            get { return this._master; }
            set { this._master = value; }
        }

        public string MasterDateTime
        {
            get { return this._masterDateTime; }
            set { this._masterDateTime = value; }
        }

        public int UserCount
        {
            get { return this._userCount; }
            set { this._userCount = value; }
        }

        public int PostCount
        {
            get { return this._postCount; }
            set { this._postCount = value; }
        }

        //public Community(int communityId, int categoryId, string communityName, string description, string image)
        //{
        //    this.CommunityId = communityId;
        //    this.CategoryId = categoryId;
        //    this.CommunityName = communityName;
        //    this.Description = description;
        //    this.Image = image;
        //}

        //community_id, community_name, category_id, description, image, community_datetime, era, era_datetime, community_master, master_datetime, user_count, post_count
        public Community(int communityId,
                         int categoryId,
                         string communityName,
                         string description,
                         string image,
                         string communityDateTime,
                         string era,
                         string eraDateTime,
                         string master,
                         string masterDateTime,
                         int userCount,
                         int postCount)
        {
            this.CommunityId = communityId;
            this.CategoryId = categoryId;
            this.CommunityName = communityName;
            this.Description = description;
            this.Image = image;
            this.CommunityDateTime = communityDateTime;
            this.Era = era;
            this.EraDateTime = eraDateTime;
            this.Master = master;
            this.MasterDateTime = masterDateTime;
            this.UserCount = userCount;
            this.PostCount = postCount;
        }

    }
}
