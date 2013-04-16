using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MultipleChoice
{
    public sealed partial class ExtendedStart : Page
    {
        public ExtendedStart()
        {
            this.InitializeComponent();
            Loaded += async (s, e) =>
            {
                if (MultipleChoice.GameViewModel.Strings.IncludeAdvertising)
                    try
                    {
                        var _Geo = new Geolocator();
                        await _Geo.GetGeopositionAsync();
                    }
                    catch
                    {
                        // this breakpoint may be reached if the user BLOCKS location services
                        System.Diagnostics.Debugger.Break();
                    }
                this.Frame.Navigate(typeof(Game));
            };
        }
    }
}
