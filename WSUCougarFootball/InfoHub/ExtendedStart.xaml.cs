using System;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;

namespace InfoHub
{
    public sealed partial class ExtendedStart : Page
    {
        public ExtendedStart()
        {
            this.InitializeComponent();
            Loaded += async (s, e) =>
            {
                if (InfoHub.AppHubViewModel.Strings.AdInclude)
                    try
                    {
                        var _Geo = new Geolocator();
                        await _Geo.GetGeopositionAsync(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5));
                    }
                    catch { Debug.WriteLine("Error reading location; user may have BLOCKED"); }
                if (AppHubViewModel.Strings.Theme == Common.Themes.Classic)
                    Frame.Navigate(typeof(AppHub_Classic));
                else
                    Frame.Navigate(typeof(AppHub_Sports));
            };
        }
    }
}
