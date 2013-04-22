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
            public const string Title = "Seattle Seahawks Fan";
            public const string SubTitle = "News, Photos, and more..";

            // Critical
            public const string SearchTerm1 = "Seattle";
            public const string SearchTerm2 = "Seahawks";
            public const string YouTubeAuthor = "GoSeattleSeaHawks12";             //YouTubeAuthor

            // Optional
            public const string TwitterUser = "Seahawks";   //Only if using TwitterUser instead of search
            public const double WeatherLatitude = 47.6097; 
            public const double WeatherLongitude = -122.3331;

            // SUMMARY / OVERVIEW SECTION

            // this "summary" is extra data to make your app more valuable
            public const bool SummaryInclude = true;
            public const string SummaryTitle = "Overview";
            public const string SummaryMoreUrl = "http://en.wikipedia.org/wiki/Seattle_Seahawks";
            public static object SummaryData = new
            {
                Title1 = "Seattle", // First line
                Title2 = "Seahawks", // Second line
                Image = "ms-appx:///Assets/SummaryImage.png", // Optional if you don't have one
                Facts = new string[][]
                { 
                    // usually room for about 7 items (fewer is okay, too  comment out lines)
                   new string[]{ "Home Field", "CenturyLink Field, Seattle, WA" },
                   new string[]{ "Owner", "Paul Allen" },
                   new string[]{ "General Manager", "John Schneider" },
                   new string[]{ "Head Coach", "Pete Carroll" },
                   new string[]{ "Last Conference Championship", "2005 (v Pittsburgh)" },
                   new string[]{ "Last Division Championship", "2010 NFC (v St Louis)" },
                   new string[]{ "Last Playoff Appearance", "2012 (v Indianapolis)" },
                },
            };

            // STANDARD NEWS FEED

            // any standard rss feed can be used here
            public const bool NewsInclude = true;
            public const string NewsTitle = "Current News";
            public const int NewsCount = 7;
            public const string NewsSourceUrl = "http://feeds.feedburner.com/SeahawksTeamNews";
            public const string NewsMoreUrl = "http://www.seahawks.com/news/index.html";

            // YOUTUBE

            // get your YouTube link here: https://gdata.youtube.com/demo/index.html
            public const bool YouTubeInclude = true;
            public const string YouTubeTitle = "Current Videos";
            public const int YouTubeCount = 5;

            public const string YouTubeSourceUrl = "http://gdata.youtube.com/feeds/base/videos?max-results=12&alt=rss&author=" + YouTubeAuthor;
            public const string YouTubeMoreUrl = "http://youtube.com/" + YouTubeAuthor;

            // PRIVACY POLICY

            // build a statement at: http://privacychoice.org
            // here's a generic policy: http://freeprivacypolicy.org/generic.php
            public const string PrivacyUrl = "http://blog.jerrynixon.com/p/liquid47.html";
            public const string PrivacyPolicy = "Privacy Policy";

            // FLICKR FEED

            // get your flickr link here: http://www.flickr.com/services/feeds/docs/photos_public/
            // example: single user http://api.flickr.com/services/feeds/photos_public.gne?id=36140829@N03&lang=en-us&format=rss_200
            // for a single user, look at the bottom of their photostream page for the rss button
            public const bool FlickrInclude = true;
            public const string FlickrTitle = "Recent Photos";
            public const int FlickrCount = 5;

            // Flickr (2 search terms)
            public const string FlickrSourceUrl = "http://api.flickr.com/services/feeds/photos_public.gne?tags=" + SearchTerm1 + "," + SearchTerm2 + "&tagmode=all&format=rss2";
            public const string FlickrMoreUrl = "http://www.flickr.com/search/?q=" + SearchTerm1 + "%20" + SearchTerm2;

            // TWITTER FEED

            // get your twitter link here: https://dev.twitter.com/docs/api/1/get/statuses/user_timeline
            // example: single user http://api.twitter.com/1/statuses/user_timeline.rss?screen_name=jerrynixon
            public const bool TwitterInclude = true;
            public const string TwitterTitle = "Latest Tweets";
            public const int TwitterCount = 12;

            // 2 Search terms
            public const string TwitterSourceUrl = "http://search.twitter.com/search.rss?q=" + SearchTerm1 + "%20" + SearchTerm2 + "&rpp=20";
            public const string TwitterMoreUrl = "https://twitter.com/search?q=" + SearchTerm1 + "%20" + SearchTerm2 + "&src=typd";

            // LOCAL WEATHER

            // get your weather link here: http://forecast.weather.gov/
            public const bool WeatherInclude = true;
            public const string WeatherTitle = "Weather";
            public const int WeatherCount = 7; 

            // CALENDAR 
            // standard ics calendar can be used here
            // set to TRUE if you find one
            public const bool CalendarInclude = true;
            public const string CalendarTitle = "Schedule";
            public const int CalendarCount = 12;
            public const string CalendarSourceUrl = "http://www.southendzone.com/ical/seahawks.ics";
            public const string CalendarMoreUrl = "http://www.seahawks.com/schedule/season-schedule.html";

            // PRIVACY POLICY & EMAIL 

            // Local Privacy Policy if Web Base is not enough - Add your email and set to true
            // can help if policy is issue passing
            public const bool PrivacyPolicyUseLocal = false;
            public const string PrivacyPolicyText = "Privacy Policy";
            public const string ContactEmail = "YourEmail@YourEmail.com";
            public const string PrivacyPolicyLocalText = "This privacy policy governs your use of this application. "
                + "The  Application does not collect personal information and does not monitor your personal use of the Application. "
                + "This application does not transmit any information without your knowledge. "
                + "You can easily uninstall the application at any time by using the standard uninstall processes available with Windows platform. "
                + "If you have a reason to believe that your personal information is being tracked or collected while using the Application, please contact us at " + ContactEmail;

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
            public const string AdApplicationId = "43da88f7-2b36-46f3-81dd-0b043193e1c6";
            public const string AdUnitId = "10056298";

            public const string NoInternetWarning = "Internet is required. Check connection and refresh.";
        }
    }
}
