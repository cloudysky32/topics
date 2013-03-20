using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Topics.Data;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace Topics.Util
{
    class HttpClientPostType : IDisposable
    {
        private HttpClient _httpClient;

        public HttpClientPostType()
        {
            HttpHelpers.CreateHttpClient(ref _httpClient);
        }

        #region Method : Sigin In, Function Code : 0
        public async Task<bool> SignIn()
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {   
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.SIGNIN.ToString()));
                postData.Add(new KeyValuePair<string, string>("user_email", User.Instance.Email));
                postData.Add(new KeyValuePair<string, string>("user_name", User.Instance.Name));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    JsonParser jsonParser = new JsonParser();
                    if (jsonParser.GetStatus(responseString))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return false;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return false;
            }
        }
        #endregion

        #region Method : Get Category List, Function Code : 1
        public async Task<List<Category>> GetCategoryList(string parentId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.CATEGORY.ToString()));
                postData.Add(new KeyValuePair<string, string>("parent_id", parentId));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Category> categoryList = jsonParser.ParseCategory(responseString);

                        if (categoryList != null)
                            return categoryList;
                        else 
                            return null;
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Get Community List, Function Code : 2
        public async Task<List<Community>> GetCommunityList(string categoryId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.COMMUNITY.ToString()));
                postData.Add(new KeyValuePair<string, string>("category_id", categoryId));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Community> communityList = jsonParser.ParseCommunity(responseString);

                        if (communityList != null)
                        {
                            return communityList;
                        } 
                        else { return null; }
                    } 
                    else { return null; }
                } 
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Get Post List, Function Code : 3
        public async Task<List<Post>> GetPostList(string communityId, string userEmail)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.POST.ToString()));
                postData.Add(new KeyValuePair<string, string>("community_id", communityId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Post> postList = jsonParser.ParsePost(responseString);

                        if (postList != null)
                        {
                            return postList;
                        }
                        else { return null; }
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Get Comment List, Function Code : 4
        public async Task<List<Comment>> GetCommentList(string postId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.COMMENT.ToString()));
                postData.Add(new KeyValuePair<string, string>("post_id", postId));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Comment> commentList = jsonParser.ParseComment(responseString);

                        if (commentList != null)
                        {
                            return commentList;
                        }
                        else { return null; }
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Get Subscription List, Function Code : 5
        public async Task<List<Community>> GetSubscriptionList(string userEmail)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.SUBSCRIPTION.ToString()));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Community> communityList = jsonParser.ParseCommunity(responseString);

                        if (communityList != null)
                            return communityList;
                        else
                            return null;
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Create Community, Function Code : 6
        public async Task<bool> CreateCommunity(string userEmail, string communityName, string description, string categoryId, StorageFile file)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                HttpContent httpContent = new ByteArrayContent(await GetPhotoBytesAsync(file));

                multipartFormDataContent.Add(httpContent, "file", file.Name);

                httpContent = new StringContent(HttpFunctionCodes.CREATE_COMMUNITY.ToString());
                multipartFormDataContent.Add(httpContent, "select");

                httpContent = new StringContent(userEmail);
                multipartFormDataContent.Add(httpContent, "user_email");

                httpContent = new StringContent(communityName);
                multipartFormDataContent.Add(httpContent, "community_name");

                httpContent = new StringContent(description);
                multipartFormDataContent.Add(httpContent, "description");

                httpContent = new StringContent(categoryId);
                multipartFormDataContent.Add(httpContent, "category_id");

                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, multipartFormDataContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.GetStatus(responseString);
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return false;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return false;
            }
        }
        #endregion

        #region Method : Submit Post, Function Code : 7
        public async Task<int> SubmitPost(string communityId, string userEmail, string content)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.SUBMIT_POST.ToString()));
                postData.Add(new KeyValuePair<string, string>("community_id", communityId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));
                postData.Add(new KeyValuePair<string, string>("content", content));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);

                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.ParseSubmittedPostId(responseString);
                    }
                    else { return 0; }
                }
                else { return 0; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return 0;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return 0;
            }
        }
        #endregion

        #region Method : Overload Submit Post, Function Code : 7
        public async Task<int> SubmitPost(string communityId, string userEmail, string content, StorageFile file)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                HttpContent httpContent = new ByteArrayContent(await GetPhotoBytesAsync(file));

                multipartFormDataContent.Add(httpContent, "file", file.Name);

                httpContent = new StringContent(HttpFunctionCodes.SUBMIT_POST.ToString());
                multipartFormDataContent.Add(httpContent, "select");

                httpContent = new StringContent(communityId);
                multipartFormDataContent.Add(httpContent, "community_id");

                httpContent = new StringContent(userEmail);
                multipartFormDataContent.Add(httpContent, "user_email");

                httpContent = new StringContent(content);
                multipartFormDataContent.Add(httpContent, "content");

                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, multipartFormDataContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.ParseSubmittedPostId(responseString);
                    }
                    else { return 0; }
                }
                else { return 0; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return 0;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return 0;
            }
        }
        #endregion

        #region Method : Submit Comment, Function Code : 8
        public async Task<bool> SubmitComment(string postId, string userEmail, string comment)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.SUBMIT_COMMENT.ToString()));
                postData.Add(new KeyValuePair<string, string>("post_id", postId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));
                postData.Add(new KeyValuePair<string, string>("comment", comment));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);

                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.GetStatus(responseString);
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return false;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return false;
            }
        }
        #endregion

        #region Method : Get Hot Topic List, Function Code: 9
        public async Task<List<Post>> GetHotTopicList(string communityId, string userEmail)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.HOT_TOPICS.ToString()));
                postData.Add(new KeyValuePair<string, string>("community_id", communityId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Post> postList = jsonParser.ParsePost(responseString);

                        if (postList != null)
                        {
                            return postList;
                        }
                        else { return null; }
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Get Weekly Topic List, Function Code : 10
        public async Task<List<Post>> GetWeeklyTopicList(string communityId, string userEmail)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.WEEKLY_TOPIC.ToString()));
                postData.Add(new KeyValuePair<string, string>("community_id", communityId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Post> postList = jsonParser.ParseWeeklyTopics(responseString);

                        if (postList != null)
                        {
                            return postList;
                        }
                        else { return null; }
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return null;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Method : Like Post, Function Code : 11
        public async Task<bool> LikePost(string postId, string userEmail, string value)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.LIKE_POST.ToString()));
                postData.Add(new KeyValuePair<string, string>("post_id", postId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));
                postData.Add(new KeyValuePair<string, string>("value", value));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);

                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.GetStatus(responseString);
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return false;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return false;
            }
        }
        #endregion

        #region Method : Subscribe Community, Function Code : 12
        public async Task<bool> Subscribe(string userEmail, string communityId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.SUBSCRIBE.ToString()));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));
                postData.Add(new KeyValuePair<string, string>("community_id", communityId));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);

                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.GetStatus(responseString);
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch (HttpRequestException exception)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + exception.Message);
                return false;
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException occured: " + exception.Message);
                return false;
            }
        }
        #endregion

        public async Task<byte[]> GetPhotoBytesAsync(StorageFile file)
        {
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            var reader = new Windows.Storage.Streams.DataReader(fileStream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)fileStream.Size);

            byte[] pixels = new byte[fileStream.Size];
            reader.ReadBytes(pixels);
            return pixels;
        }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }
    }
}
