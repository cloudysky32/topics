using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Util
{
    class HttpFunctionCodes
    {
        public const string         URL = "http://topics.azurewebsites.net/";
        public const string         RESOURCE_ADDRESS = "topics.php";

        public const int            SIGNIN = 0;
        public const int            CATEGORY = 1;
        public const int            COMMUNITY = 2;
        public const int            POST = 3;
        public const int            COMMENT = 4;

        public const int            SUBSCRIPTION = 5;
        public const int            CREATE_COMMUNITY = 6;
        public const int            SUBMIT_POST = 7;
        public const int            SUBMIT_COMMENT = 8;

        public const int            HOT_TOPICS = 9;
        public const int            WEEKLY_TOPIC = 10;

        public const int            LIKE_POST = 11;
        public const int            SUBSCRIBE = 12;

        public const int            TEST = 99;
    }
}
