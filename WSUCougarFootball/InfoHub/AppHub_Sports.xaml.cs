using InfoHub.Articles;
using InfoHub.Common;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace InfoHub
{
    public sealed partial class AppHub_Sports : LayoutAwarePage
    {
        public AppHub_Sports()
        {
            this.InitializeComponent();
            Loaded += AppHub_Loaded;
        }

        /// <summary>
        /// Highlighted article clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StackPanel_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            // go to highlighted
            await Windows.System.Launcher.LaunchUriAsync(new Uri(this.ViewModel.HighlightedArticle.Url));
        }

        DispatcherTimer m_Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        async void AppHub_Loaded(object sender, RoutedEventArgs e)
        {
            m_Timer.Tick += (s, args) =>
            {
                m_Timer.Stop();
                MyBottomAppBar.IsOpen = false;
            };
            m_Timer.Start();

            GridViewOut.ItemsSource = cvs.View.CollectionGroups;
            await ViewModel.Start();


            // are we enabling advertising?
            if (!AppHubViewModel.Strings.AdInclude)
                this.MyPurchaseButton.Visibility = Visibility.Collapsed;
            else
            {
                ViewModel.PropertyChanged += (s, args) =>
                {
                    if (!new string[] { "Longitude", "Latitude" }.Contains(args.PropertyName))
                        return;
                    UpdateAds();
                };
                this.ViewModel.Feeds.CollectionChanged += (s, args) =>
                {
                    foreach (var item in this.ViewModel.Feeds)
                    {
                        item.Articles.CollectionChanged -= Articles_CollectionChanged;
                        item.Articles.CollectionChanged += Articles_CollectionChanged;
                    }
                    UpdateAds();
                };
                UpdateAds();
            }

        }

        void Articles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) { UpdateAds(); }

        /// <summary>
        /// FillView content clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void HubGridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            // can't get more ifthere's no internet
            if (await IsInternet())
            {
                var _Item = e.ClickedItem as IArticle;
                if (_Item is AdvertArticle)
                    return;
                else if (_Item is SummaryArticle)
                    return;
                else if (_Item is CalendarArticle)
                    return;
                else if (_Item is WeatherArticle)
                    return;
                else
                {
                    var _AppHubViewModel = this.DataContext as AppHubViewModel;
                    if (_AppHubViewModel != null)
                    {
                        SuspendAds();
                        var _Feed = _AppHubViewModel
                            .Feeds.First(x => x.Articles.Contains(_Item)).Feed;
                        MyBrowser.Navigate(_Item, _Feed);
                    }
                }
            }
        }

        /// <summary>
        /// SnapView content clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void ListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                var _Article = e.ClickedItem as IArticle;
                if (_Article != null)
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(_Article.Url));
            }
            catch { }
        }

        /// <summary>
        /// Summary clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ContentControl_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                var _Control = sender as ContentControl;
                dynamic _Data = _Control.Content;
                await Windows.System.Launcher.LaunchUriAsync(new Uri(_Data.Link));
            }
            catch { }
        }

        // utility

        AppHubViewModel ViewModel { get { return this.DataContext as AppHubViewModel; } }

        List<AdControl> Ads
        {
            get
            {
                List<AdControl> _List = new List<AdControl>();
                foreach (var _Item in HubGridView.Items)
                {
                    var _Container = HubGridView.ItemContainerGenerator.ContainerFromItem(_Item);
                    var _Children = AllChildren(_Container);
                    var _AdControls = _Children.OfType<Microsoft.Advertising.WinRT.UI.AdControl>();
                    _List.AddRange(_AdControls);
                }
                return _List;
            }
        }

        void SuspendAds()
        {
            foreach (var item in this.Ads)
                item.Suspend();
        }

        void ResumeAds()
        {
            foreach (var item in this.Ads)
                item.Resume();
        }

        void UpdateAds()
        {
            foreach (var item in this.Ads)
            {
                item.Latitude = this.ViewModel.Latitude;
                item.Longitude = this.ViewModel.Longitude;
                item.ApplicationId = AppHubViewModel.Strings.AdApplicationId;
                item.AdUnitId = AppHubViewModel.Strings.AdUnitId;
            }
        }

        public List<Control> AllChildren(DependencyObject parent)
        {
            if (parent == null)
                return (new Control[] { }).ToList();
            var _List = new List<Control> { };
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }

        public static async System.Threading.Tasks.Task<bool> IsInternet()
        {
            var _InternetConnectionProfile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
            var _IsInernet = _InternetConnectionProfile != null;
            if (!_IsInernet)
                await new Windows.UI.Popups.MessageDialog(
                    content: InfoHub.AppHubViewModel.Strings.NoInternetWarning,
                    title: InfoHub.AppHubViewModel.Strings.NoInternetWarning).ShowAsync();
            return _IsInernet;
        }
    }
}

