using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Topics.Data
{
    public sealed class User
    {
        private static volatile User _instance;
        private static object _syncRoot = new Object();

        public string Email { get; set; }
        public string Name { get; set; }
        public string PictureUri { get; set; }

        private User() { }

        public static User Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new User();
                    }
                }

                return _instance;
            }
        }
    }
}
