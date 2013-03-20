using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Data;

namespace Topics.Util
{
    public class RoamingSetting
    {
        static Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        public static bool InitUser()
        {
            if (roamingSettings.Values.ContainsKey("userEmail"))
            {
                User.Instance.Email = roamingSettings.Values["userEmail"].ToString();
                User.Instance.Name = roamingSettings.Values["userName"].ToString();
                User.Instance.PictureUri = roamingSettings.Values["userPictureUri"].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool StoreUserData()
        {
            try
            {
                roamingSettings.Values["userEmail"] = User.Instance.Email;
                roamingSettings.Values["userName"] = User.Instance.Name;
                roamingSettings.Values["userPictureUri"] = User.Instance.PictureUri;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + ex.Message);
                return false;
            }

            return true;
        }

        public static bool RemoveUserData()
        {
            try
            {
                roamingSettings.Values.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("HttpRequestException occured: " + ex.Message);
                return false;
            }

            return true;
        }
    }
}
