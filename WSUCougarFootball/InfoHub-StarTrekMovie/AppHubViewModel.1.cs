using InfoHub.Articles;
using InfoHub.Feeds;
using InfoHub.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoHub
{
    public partial class AppHubViewModel
    {
        public static class Strings
        {
            // MAIN INFORMATION

            // this is the title and subtitle of the main page (above the grid)
            public const string Title = "Star Trek Movie Fan";
            public const string SubTitle = "News, Photos, and more..";

            public const int NewsCount = 8;
            public const int YouTubeCount = 8;
            public const int FlickrCount = 5;
            public const int TwitterCount = 12;

            // 0 == title of article
            // 1 == author of article
            // 2 == date of article
            // 3 == url of article
            // 4 == url of feed
            public const string ShareHtml = "Hey! <p>I wanted to share {0} by {1} on {2:d}. <p>It's from {3} in {4}. <p>Check it out!";
            public const string ShareTitle = "Information";

            // set to false to hide ads everywhere hardcoded
            public static bool IncludeAdvertising = true; // in general this should be left to true
            public static bool SimulatePurchasing = System.Diagnostics.Debugger.IsAttached;

            // get your ad values from http://pubcenter.microsoft.com
            public const string AdApplicationId = "1ae6cc78-0428-458c-a623-736c0134afb7";
            public const string AdUnitId = "10065322";

            public const string NoInternetWarning = "Internet is required. Check connection and refresh.";

            public static string PrivacyPolicy = "Privacy Policy";
            public static string PrivacyUrl = "http://blog.jerrynixon.com/p/liquid47.html";
        }
    }
}
