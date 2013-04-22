using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoHub.Common
{
    public enum Themes { Classic, Sports }
    public class StringsModel
    {
        public Themes Theme = Themes.Classic;

        // Title and subtitle of the main page (above the grid)
        public string Title = string.Empty;
        public string SubTitle = string.Empty;

        /*
         * SUMMARY
         * note: the IMAGE property is optional, it's extra decoration for the first tile
         * note: the MORE url is where the user is sent when they click the text area
         */
        public bool SummaryInclude = false;
        public object SummaryData = null;

        /*
         * NEWS
         * reference: mix feeds: http://chimpfeedr.com/
         * reference: create a feed: http://www.feed43.com/
         * example: http://www.denverbroncos.com/cda-web/rss-module.htm?tagName=News
         */
        public int NewsCount = 7;
        public bool NewsInclude = false;
        public string NewsTitle = string.Empty;
        public string NewsSourceUrl = string.Empty;
        public string NewsMoreUrl = string.Empty;

        /*
         * TWITTER
         * reference: https://dev.twitter.com/docs/api/1/get/statuses/user_timeline
         * example: single user http://api.twitter.com/1/statuses/user_timeline.rss?screen_name=jerrynixon
         * example: search term http://search.twitter.com/search.rss?q=Broncos&rpp=20
         * example: search terms "http://search.twitter.com/search.rss?q=Denver%20Broncos&rpp=20
         */
        public int TwitterCount = 12;
        public bool TwitterInclude = false;
        public string TwitterTitle = string.Empty;
        public string TwitterSourceUrl = string.Empty;
        public string TwitterMoreUrl = string.Empty;

        /* 
         * YOUTUBE
         * reference: https://gdata.youtube.com/demo/index.html
         * example: channel feed: http://gdata.youtube.com/feeds/api/users/BRONCOPLANET/uploads
         * example: user feed: http://gdata.youtube.com/feeds/base/videos?max-results=12&alt=rss&author=JERRYNIXON
         * exmaple: search term feed: http://gdata.youtube.com/feeds/base/videos?alt=rss&q=Broncos
         * exmaple: search terms feed: http://gdata.youtube.com/feeds/base/videos?alt=rss&q=Denver%20Broncos
         */
        public int YouTubeCount = 5;
        public bool YouTubeInclude = false;
        public string YouTubeTitle = string.Empty;
        public string YouTubeSourceUrl = string.Empty;
        public string YouTubeMoreUrl = string.Empty;

        /*
         * FLICKR FEED
         * resource: http://www.flickr.com/services/feeds/docs/photos_public/
         * example: user feed: http://api.flickr.com/services/feeds/photos_public.gne?id=36140829@N03&lang=en-us&format=rss_200
         * note: for a single user, look at the bottom of their photostream page for the rss button
         * example: search term feed: "http://api.flickr.com/services/feeds/photos_public.gne?tags=Broncos&tagmode=all&format=rss2"
         * example: search terms feed: "http://api.flickr.com/services/feeds/photos_public.gne?tags=Denver,Broncos&tagmode=all&format=rss2"
         */
        public int FlickrCount = 5;
        public bool FlickrInclude = false;
        public string FlickrTitle = string.Empty;
        public string FlickrSourceUrl = string.Empty;
        public string FlickrMoreUrl = string.Empty;

        /*
         * PRIVACY POLICY
         * You must have a privacy policy if you use the internet (this app uses the internet)
         * reference: build your own: http://privacychoice.org
         * reference: generic policy: http://freeprivacypolicy.org/generic.php
         * note: local policies are not needed unless the tester specifies you need one
         */
        public string PrivacyPolicyUrl = "http://freeprivacypolicy.org/generic.php";
        public string PrivacyPolicyText = "Privacy Policy";
        public bool PrivacyPolicyUseLocal = false;
        public string PrivacyPolicyLocalText = "This privacy policy governs your use of this application. "
            + "The  Application does not collect personal information and does not monitor your personal use of the Application. "
            + "This application does not transmit any information without your knowledge. "
            + "You can easily uninstall the application at any time by using the standard uninstall processes available with Windows platform. "
            + "If you have a reason to believe that your personal information is being tracked or collected while using the Application, "
            + "please contact us.";

        /*
         * CALENDAR
         * note: standard ics calendar only
         */
        public int CalendarCount = 12;
        public bool CalendarInclude = false;
        public string CalendarTitle = string.Empty;
        public string CalendarSourceUrl = string.Empty;
        public string CalendarMoreUrl = string.Empty;

        /*
         * WEATHER
         * reference: get weather: http://forecast.weather.gov/
         * reference: get lat/long: http://weather.gov/
         * note: there cannot be more than 7 days
         * note: this is for USA weather only
         */
        public int WeatherCount = 7;
        public bool WeatherInclude = false;
        public string WeatherTitle = string.Empty;
        public double WeatherLatitude = 0;
        public double WeatherLongitude = 0;

        // 0 == title of article
        // 1 == author of article
        // 2 == date of article
        // 3 == url of article
        // 4 == url of feed
        public string ShareHtml = "Hey! <p>I wanted to share {0} by {1} on {2:d}. <p>It's from {3} in {4}. <p>Check it out!";
        public string ShareTitle = "Information";
        public string NoInternetWarning = "Internet is required. Check connection and refresh.";

        /*
         * ADVERTISING
         * reference: http://pubcenter.microsoft.com
         */
        public bool AdInclude = false; // in general this should be left to true
        public bool AdSimulatePurchasing = System.Diagnostics.Debugger.IsAttached;
        public string AdApplicationId = string.Empty;
        public string AdUnitId = string.Empty;
    }
}
