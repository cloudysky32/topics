using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Live;
using Windows.UI.Xaml.Controls;
using Topics.Data;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace Topics.Util
{
    public class OAuthLiveId
    {
        public static async Task<bool> GetUserAccount()
        {
            try
            {
                // Open Live Connect SDK client.
                LiveAuthClient LCAuth = new LiveAuthClient();
                LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();
                try
                {
                    LiveLoginResult loginResult = await LCAuth.LoginAsync(new string[] { "wl.emails" });

                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        LiveConnectClient connect = new LiveConnectClient(LCAuth.Session);
                        LiveOperationResult operationResult = await connect.GetAsync("me");
                        dynamic result = operationResult.Result;

                        if (result != null)
                        {
                            // Update the text of the object passed in to the method. 
                            User.Instance.Email = result.emails.account;
                            User.Instance.Name = result.name;

                            operationResult = await connect.GetAsync("me/picture");

                            if (result != null)
                            {
                                result = operationResult.Result;
                                User.Instance.PictureUri = result.location;
                            }

                            return true;
                        }
                        else
                        {
                            // Handle the case where the user name was not returned.
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (LiveAuthException exception)
                {
                    System.Diagnostics.Debug.WriteLine("LiveAuthException occured: " + exception.Message);
                    return false;
                }
            }
            catch (LiveAuthException exception)
            {
                System.Diagnostics.Debug.WriteLine("LiveAuthException occured: " + exception.Message);
                return false;
            }
            catch (LiveConnectException exception)
            {
                System.Diagnostics.Debug.WriteLine("LiveConnectException occured: " + exception.Message);
                return false;
            }
        }

        public static async Task<bool> SignOut()
        {
            try
            {
                LiveAuthClient LCAuth = new LiveAuthClient();
                LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();

                if (LCLoginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    LCAuth.Logout();
                    return true;
                }

                return false;
            }
            catch (LiveConnectException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
