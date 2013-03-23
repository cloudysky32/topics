using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Windows.UI.Xaml.Controls;
using Topics.Data;
#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

namespace Topics.Util
{
    class JsonParser
    {
        #region Method : Parse Category
        public List<Category> ParseCategory(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    try
                    {
                        JToken jtoken = root.SelectToken("result");
                        List<dynamic> resultList = jtoken.ToList<dynamic>();
                        List<Category> categoryList = new List<Category>();

                        foreach (dynamic result in resultList)
                        {
                            categoryList.Add(new Category((int)result.categoryId, (string)result.categoryName, (int)result.parentId, (string)result.imageUri));
                        }

                        return categoryList;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Parse Community
        public List<Community> ParseCommunity(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    JToken jtoken = root.SelectToken("result");

                    try
                    {
                        List<dynamic> resultList = jtoken.ToList<dynamic>();
                        List<Community> communityList = new List<Community>();

                        //community_id, community_name, category_id, description, image, community_datetime, era, era_datetime, community_master, master_datetime, user_count, post_count
                        foreach (dynamic result in resultList)
                        {
                            communityList.Add(new Community((int)result.communityId,
                                                    (int)result.categoryId,
                                                    (string)result.communityName,
                                                    (string)result.description,
                                                    (string)result.image,
                                                    (string)result.communityDateTime.date,
                                                    (string)result.era,
                                                    (string)result.eraDateTime.date,
                                                    (string)result.communityMaster,
                                                    (string)result.masterDateTime.date,
                                                    (int)result.userCount,
                                                    (int)result.postCount));
                        }

                        return communityList;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Parse Post
        public List<Post> ParsePost(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    JToken jtoken = root.SelectToken("result");

                    try
                    {
                        List<dynamic> resultList = jtoken.ToList<dynamic>();
                        List<Post> postList = new List<Post>();

                        foreach (dynamic result in resultList)
                        {
                            postList.Add(new Post((int)result.postId,
                                                (int)result.communityId,
                                                (string)result.userEmail,
                                                (string)result.content,
                                                (string)result.imageUri,
                                                (string)result.likeCount,
                                                (bool)result.value,
                                                (string)result.dateTime.date));
                        }

                        return postList;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Parse Weekly Topics
        public List<Post> ParseWeeklyTopics(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    JToken jtoken = root.SelectToken("result");

                    try
                    {
                        List<dynamic> resultList = jtoken.ToList<dynamic>();
                        List<Post> postList = new List<Post>();

                        foreach (dynamic result in resultList)
                        {
                            postList.Add(new Post((int)result.postId,
                                                    (int)result.communityId,
                                                    (string)result.userEmail,
                                                    (string)result.content,
                                                    (string)result.imageUri,
                                                    (string)result.likeCount,
                                                    (bool)result.value,
                                                    (string)result.dateTime.date,
                                                    (string)result.weeklyTopics.date,
                                                    (int)result.week));
                        }

                        return postList;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Parse Comment
        public List<Comment> ParseComment(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    JToken jtoken = root.SelectToken("result");

                    try
                    {
                        List<dynamic> resultList = jtoken.ToList<dynamic>();
                        List<Comment> commentList = new List<Comment>();

                        foreach (dynamic result in resultList)
                        {
                            commentList.Add(new Comment((int)result.commentId,
                                                        (int)result.postId,
                                                        (string)result.userEmail,
                                                        (string)result.comment,
                                                        (string)result.dateTime.date));
                        }

                        return commentList;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Parse Submitted Post Id
        public int ParseSubmittedPostId(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    JToken jtoken = root.SelectToken("result");

                    try
                    {
                        List<dynamic> resultList = jtoken.ToList<dynamic>();

                        return resultList[0].issueId;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
                return 0;
            }
        }
        #endregion

        #region Get Query Status
        public bool GetStatus(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if ((bool)root.SelectToken("status"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (JsonReaderException exception)
            {
                System.Diagnostics.Debug.WriteLine("JsonReaderException occured: " + exception.Message);
                return false;
            }
        }
        #endregion
    }
}
