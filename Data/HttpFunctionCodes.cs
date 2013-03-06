using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Data
{
    class HttpFunctionCodes
    {
        public const string         URL = "http://topics.azurewebsites.net/";
        public const string         RESOURCE_ADDRESS = "topics.php";

        public const int            SIGNIN = 0;
        public const int            CATEGORY = 1;
        public const int            TOPIC = 2;
        public const int            ISSUE = 3;
        public const int            COMMENT = 4;

        public const int            SUBSCRIPTION = 5;
        public const int            CREATE_TOPIC = 6;
        public const int            POST_ISSUE = 7;
        public const int            POST_COMMENT = 8;

        public const int            HOT_ISSUE = 9;

        public const int            TEST = 99;
    }
}
