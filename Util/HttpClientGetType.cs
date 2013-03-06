using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Topics.Data;

namespace Topics.Util
{
    class HttpClientGetType : IDisposable
    {
        private HttpClient _httpClient;

        public HttpClientGetType()
        {
            _httpClient = new HttpClient();
        }

        public async Task SignIn(TextBlock NotifyField)
        {
            // The value of 'AddressField' is set by the user and is therefore untrusted input. If we can't create a
            // valid, absolute URI, we'll notify the user about the incorrect input.
            // Note that this app has both "Internet (Client)" and "Home and Work Networking" capabilities set,
            // since the user may provide URIs for servers located on the intErnet or intrAnet. If apps only
            // communicate with servers on the intErnet, only the "Internet (Client)" capability should be set.
            // Similarly if an app is only intended to communicate on the intrAnet, only the "Home and Work
            // Networking" capability should be set.

            string url = string.Join("", HttpFunctionCodes.URL, HttpFunctionCodes.RESOURCE_ADDRESS, "?select=", HttpFunctionCodes.SIGNIN, "&user_email=", User.Instance.Email);

            Uri resourceUri;

            if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out resourceUri))
            {
                //rootPage.NotifyUser("Invalid URI.", NotifyType.ErrorMessage);
                NotifyField.Text = string.Join("Invalid URI", url);
                return;
            }

            //HttpHelpers.ScenarioStarted(StartButton, CancelButton);
            //rootPage.NotifyUser("In progress", NotifyType.StatusMessage);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(resourceUri);

                //await HttpHelpers.DisplayTextResult(response, NotifyField);
            }
            catch (HttpRequestException hre)
            {
                NotifyField.Text = hre.ToString();
            }
            catch (TaskCanceledException)
            {
                //rootPage.NotifyUser("Request canceled.", NotifyType.ErrorMessage);
                NotifyField.Text = "Request cancled";
            }
            finally
            {
                //HttpHelpers.ScenarioCompleted(StartButton, CancelButton);
            }
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
