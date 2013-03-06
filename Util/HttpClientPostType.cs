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

        #region Method : Get Topic List, Function Code : 2
        public async Task<List<Topic>> GetTopicList(string categoryId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.TOPIC.ToString()));
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
                        List<Topic> topicList = jsonParser.ParseTopic(responseString);

                        if (topicList != null)
                        {
                            return topicList;
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

        #region Method : Get Issue List, Function Code : 3
        public async Task<List<Issue>> GetIssueList(string topicId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.ISSUE.ToString()));
                postData.Add(new KeyValuePair<string, string>("topic_id", topicId));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Issue> issueList = jsonParser.ParseIssue(responseString);

                        if (issueList != null)
                        {
                            return issueList;
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

        #region Method : Get Subscription List, Function Code : 4
        public async Task<List<Topic>> GetSubscriptionList(string userEmail)
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
                        List<Topic> topicList = jsonParser.ParseTopic(responseString);

                        if (topicList != null)
                            return topicList;
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

        #region Method : Create Topic, Function Code : 5
        public async Task<bool> CreateTopic(string userEmail, string topicName, string description, string categoryId)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.CREATE_TOPIC.ToString()));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));
                postData.Add(new KeyValuePair<string, string>("topic_name", topicName));
                postData.Add(new KeyValuePair<string, string>("description", description));
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

        #region Method : Post Issue, Function Code : 7
        public async Task<int> PostIssue(string topicId, string userEmail, string content)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.POST_ISSUE.ToString()));
                postData.Add(new KeyValuePair<string, string>("topic_id", topicId));
                postData.Add(new KeyValuePair<string, string>("user_email", userEmail));
                postData.Add(new KeyValuePair<string, string>("content", content));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                //HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);

                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();

                        return jsonParser.ParsePostedIssueId(responseString);
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

        #region Method : Overload Post Issue, Function Code : 7
        public async Task<int> PostIssue(string topicId, string userEmail, string content, StorageFile file)
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                HttpContent httpContent = new ByteArrayContent(await GetPhotoBytesAsync(file));

                multipartFormDataContent.Add(httpContent, "file", file.Name);

                httpContent = new StringContent(HttpFunctionCodes.POST_ISSUE.ToString());
                multipartFormDataContent.Add(httpContent, "select");

                httpContent = new StringContent(topicId);
                multipartFormDataContent.Add(httpContent, "topic_id");

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

                        return jsonParser.ParsePostedIssueId(responseString);
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

        #region Method : Get Hot Issue List, Function Code: 9
        public async Task<List<Issue>> GetHotIssueList()
        {
            Uri baseAddress = new Uri(HttpFunctionCodes.URL);
            _httpClient.BaseAddress = baseAddress;

            try
            {
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("select", HttpFunctionCodes.HOT_ISSUE.ToString()));

                HttpContent httpContent = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _httpClient.PostAsync(HttpFunctionCodes.RESOURCE_ADDRESS, httpContent);
                Dispose();

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!responseString.Equals(""))
                    {
                        JsonParser jsonParser = new JsonParser();
                        List<Issue> issueList = jsonParser.ParseIssue(responseString);

                        if (issueList != null)
                        {
                            return issueList;
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
