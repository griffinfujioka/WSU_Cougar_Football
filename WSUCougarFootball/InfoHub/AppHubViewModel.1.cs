/*
 * WARNING this file is generated automatically
 * The values for this file are in "AppHubViewModel.xml"
 * Every time generation occurs the contents of this file are rewritten
 * Any custom changes you make in this file will be lost upon generation
 * 
 * To invoke generation, right-click "AppHubViewModel.1.tt" and select "Run Custom Tool"
 * To prevent generation, right-click "AppHubViewModel.1.tt" and select "Properties"
 *    In the properties dialog, clear the value of "Custom Tool" to blank
 *    
 * Last generation: 4/9/2013 7:26:52 PM
 */

	using InfoHub.Articles;
	using InfoHub.Common;
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
        public readonly static InfoHub.Common.StringsModel Strings = new InfoHub.Common.StringsModel()
        {
            /*
             * SECTION 1 :: SETTINGS
             * Title and subtitle of the main page (above the grid)
             */
            Title = "Cougar Football",
            SubTitle = "News, Photos, and More...",
			Theme = Themes.Classic,

            /*
             * SECTION 2 :: SUMMARY
             * note: the IMAGE property is optional, it's extra decoration for the first tile
             * note: the MORE url is where the user is sent when they click the text area
             */
			            SummaryInclude = true,
            SummaryData = new
            {
				Title1 = "Go Cougs!",
				Link = "http://www.wsucougars.com/sports/m-footbl/wast-m-footbl-body.html",
                Facts = new string[][]
                { 
                   new string[]{ "The Cougs were 3-9 overall last season.", "" },
                   new string[]{ "", "" },
                   new string[]{ "", "" },
                   new string[]{ "", "" },
                   new string[]{ "", "" },
                   new string[]{ "", "" },
                   new string[]{ "", "" },
                },
            },

            /*
             * SECTION 3 :: COUNTS
             */
			NewsCount = 7,
			FlickrCount = 5,
			TwitterCount = 12,
			YouTubeCount = 5,
			CalendarCount = 12,
			WeatherCount = 7,

            /*
             * SECTION 4 :: ADVERTISING
             * reference: http://pubcenter.microsoft.com
             * note: be sure to include "HideAds" for in-app purchase
             */
			AdInclude = false,
			AdApplicationId = "43da88f7-2b36-46f3-81dd-0b043193e1c6",
			AdUnitId = "10067184",

            /*
             * SECTION 5 :: PRIVACY POLICY
             * You must have a privacy policy if you use the internet (this app uses the internet)
             * reference: build your own: http://privacychoice.org
             * reference: generic policy: http://freeprivacypolicy.org/generic.php
             * note: local policies are not needed unless the tester specifies you need one
             */
			PrivacyPolicyUseLocal = false,
			PrivacyPolicyText = "Privacy Policy",
			PrivacyPolicyUrl = "http://freeprivacypolicy.org/generic.php",

            /*
             * SECTION 6 :: DIALOGS TEXT
             * note: these are included to allow for localization in a single place
             */
            ShareHtml = "Hey! <p>I wanted to share {0} by {1} on {2:d}. <p>It's from {3} in {4}. <p>Check it out!",
            ShareTitle = "Information",
            NoInternetWarning = "Internet is required. Check connection and refresh.",
            PrivacyPolicyLocalText = "This privacy policy governs your use of this application. The Application does not collect personal information and does not monitor your personal use of the Application. This application does not transmit any information without your knowledge. You can easily uninstall the application at any time by using the standard uninstall processes available with Windows platform. If you have a reason to believe that your personal information is being tracked or collected while using the Application, please contact us. ",
        };

        void AddFeeds()
        {
            /*
             * NEWS
             * reference: mix feeds: http://chimpfeedr.com/
             * reference: create a feed: http://www.feed43.com/
             * example: http://www.denverbroncos.com/cda-web/rss-module.htm?tagName=News
			 * example: https://news.google.com/news/feeds?q=denver+broncos&output=rss
			 * example: http://www.bing.com/news/search?q=denver+broncos&format=RSS
             */
			this.Feeds.Add(new GroupedItem(new RssFeed<NewsArticle>
            {
                Title = "Recent News",
                SourceUrl = "http://www.bing.com/news/search?q=Washington State University+Football&format=RSS",
                MoreUrl = "http://www.bing.com/news/search?q=Washington State University+Football",
            })); 

            /*
             * FLICKR FEED
             * resource: http://www.flickr.com/services/feeds/docs/photos_public/
             * example: user feed: http://api.flickr.com/services/feeds/photos_public.gne?id=36140829@N03&lang=en-us&format=rss_200
             * note: for a single user, look at the bottom of their photostream page for the rss button
             * example: search term feed: "http://api.flickr.com/services/feeds/photos_public.gne?tags=Broncos&tagmode=all&format=rss2"
             * example: search terms feed: "http://api.flickr.com/services/feeds/photos_public.gne?tags=Denver,Broncos&tagmode=all&format=rss2"
             */
			this.Feeds.Add(new GroupedItem(new RssFeed<FlickrArticle>
            {
                Title = "Popular Photos",
                SourceUrl = "http://api.flickr.com/services/feeds/photos_public.gne?tags=Washington State University,Football&tagmode=all&format=rss2",
                MoreUrl = "http://www.flickr.com/search/?q=Washington State University%20Football",
            }));

            /*
             * TWITTER
             * reference: https://dev.twitter.com/docs/api/1/get/statuses/user_timeline
             * example: single user http://api.twitter.com/1/statuses/user_timeline.rss?screen_name=jerrynixon
             * example: search term http://search.twitter.com/search.rss?q=Broncos&rpp=20
             * example: search terms "http://search.twitter.com/search.rss?q=Denver%20Broncos&rpp=20
             */
			this.Feeds.Add(new GroupedItem(new RssFeed<TwitterArticle>
            {
                Title = "Latest Tweets",
                SourceUrl = "http://search.twitter.com/search.rss?q=Washington State University%20Football&rpp=20",
                MoreUrl = "https://twitter.com/search?q=Washington State University%20Football",
            }));

            /* 
             * YOUTUBE
             * reference: https://gdata.youtube.com/demo/index.html
             * example: channel feed: http://gdata.youtube.com/feeds/api/users/BRONCOPLANET/uploads
             * example: user feed: http://gdata.youtube.com/feeds/base/videos?max-results=12&alt=rss&author=JERRYNIXON
             * exmaple: search term feed: http://gdata.youtube.com/feeds/base/videos?alt=rss&q=Broncos
             * exmaple: search terms feed: http://gdata.youtube.com/feeds/base/videos?alt=rss&q=Denver%20Broncos
             */
			this.Feeds.Add(new GroupedItem(new RssFeed<YouTubeArticle>
            {
                Title = "Current Videos",
                SourceUrl = "http://gdata.youtube.com/feeds/base/videos?alt=rss&q=Washington State University%20Football",
                MoreUrl = "http://www.youtube.com/results?search_query=Washington State University%20Football",
            }));

            /*
             * CALENDAR
             * note: standard ics calendar only
             */
			this.Feeds.Add(new GroupedItem(new CalendarFeed
            {
                Title = "Calendar",
                SourceUrl = "https://www.google.com/calendar/ical/pi5a1nnmgjmodqc3u5uusjafng%40group.calendar.google.com/public/basic.ics",
                MoreUrl = "http://www.wsucougars.com/sports/m-footbl/sched/wast-m-footbl-sched.html",
            }));

            /*
             * WEATHER
             * reference: get weather: http://forecast.weather.gov/
             * reference: get lat/long: http://weather.gov/
             * note: there cannot be more than 7 days
             * note: this is for USA weather only
             */
			this.Feeds.Add(new GroupedItem(new WeatherFeed
            {
                Title = "Weather",
                SourceUrl = "http://forecast.weather.gov/MapClick.php?lat=46.74d&lon=-117.7d&unit=0&lg=english&FcstType=dwml",
                MoreUrl = "http://forecast.weather.gov/MapClick.php?lat=46.74d&lon=-117.7d",
            }));
        }
    }
}

