using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Topics.Util
{
    class NetworkStatus
    {
        public static bool Check()
        {
            try
            {
                ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                return (internetConnectionProfile != null && internetConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("exception occured :" + exception.Message);
                return false;
            }
        }
    }
}
