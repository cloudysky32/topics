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
        //public string GetUser(string responseString)
        //{
        //    dynamic json = JsonConvert.DeserializeObject(responseString);
        //    if ((int)json.status == 1)
        //    {
        //        return json.result[1].name;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //#region Get User Method
        //public string GetUsers(string json)
        //{
        //    try
        //    {
        //        JObject root = JObject.Parse(json);
        //        if ((bool)root.SelectToken("status"))
        //        {
        //            JToken jtoken = root.SelectToken("result");
        //            List<dynamic> users = jtoken.ToList<dynamic>();

        //            return users[0].name;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (JsonReaderException exception)
        //    {
        //        System.Diagnostics.Debug.WriteLine("JsonReaderException occured :" + exception.Message);
        //        return null;
        //    }
        //}
        //#endregion

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

        public List<Topic> ParseTopic(string json)
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
                        List<Topic> topicList = new List<Topic>();

                        foreach (dynamic result in resultList)
                        {
                            topicList.Add(new Topic((int)result.topicId,
                                                    (int)result.categoryId,
                                                    (string)result.topicName,
                                                    (string)result.description,
                                                    (string)result.imageUri));
                        }

                        return topicList;
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

        public List<Issue> ParseIssue(string json)
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
                        List<Issue> issueList = new List<Issue>();

                        foreach (dynamic result in resultList)
                        {
                            issueList.Add(new Issue((int)result.issueId,
                                                    (int)result.topicId,
                                                    (string)result.userEmail,
                                                    (string)result.content,
                                                    (string)result.imageUri,
                                                    (int)result.hot,
                                                    (string)result.hot));
                        }

                        return issueList;
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

        public int ParsePostedIssueId(string json)
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
