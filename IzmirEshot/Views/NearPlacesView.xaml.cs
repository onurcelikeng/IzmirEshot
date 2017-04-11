using IzmirEshot.Models;
using System;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace IzmirEshot.Views
{
    public sealed partial class NearPlacesView : Page
    {
        private static Geopoint currentLocation;


        public NearPlacesView()
        {
            this.InitializeComponent();
            this.Loaded += NearPlacesView_Loaded;
        }


        private async void NearPlacesView_Loaded(object sender, RoutedEventArgs e)
        {
            map.MapElements.Clear();

            if(currentLocation == null)
            {
                GetLocation();
            }

            else
            {
                BasicGeoposition position = new BasicGeoposition()
                {
                    Latitude = currentLocation.Position.Latitude,
                    Longitude = currentLocation.Position.Longitude
                };
                var pin = new MapIcon()
                {
                    Location = new Geopoint(position),
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/pin.png")),
                    NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 }
                };

                map.MapElements.Add(pin);
                await map.TrySetViewAsync(new Geopoint(position), 16, 0, 0, MapAnimationKind.Bow);

                GetNearStationsRequest(currentLocation);
            }
        }

        private async void GetLocation()
        {
            progress.IsIndeterminate = true;

            try
            {
                var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
                var location = await gl.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));
                currentLocation = location.Coordinate.Point;

                var pin = new MapIcon()
                {
                    Location = location.Coordinate.Point,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/pin.png")),
                    NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 }
                };

                map.MapElements.Add(pin);
                await map.TrySetViewAsync(location.Coordinate.Point, 16, 0, 0, MapAnimationKind.Bow);

                GetNearStationsRequest(currentLocation);
            }

            catch (Exception)
            {
                await new MessageDialog("Lütfen ayarlardan konumuzunu açarak tekrar deneyiniz.", "Konum bulunamadı.").ShowAsync();
            }

            progress.IsIndeterminate = false;
        }

        private async void GetNearStationsRequest(Geopoint geo)
        {
            progress.IsIndeterminate = true;

            try
            {
                var response = await App.Client.GetNearStations(geo.Position.Latitude, geo.Position.Longitude);

                if (response.Count != 0)
                {
                    for (int i = 0; i < response.Count; i++)
                    {
                        PinToMap(response[i]);
                    }
                }

                stationListView.ItemsSource = response;
                line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private void PinToMap(StationModel model)
        {
            BasicGeoposition position = new BasicGeoposition()
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            MapIcon icon = new MapIcon()
            {
                Title = model.Name,
                Location = new Geopoint(position),
                NormalizedAnchorPoint = new Windows.Foundation.Point() { X = 0.32, Y = 0.78 },
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Icons/stationpin.png", UriKind.RelativeOrAbsolute))
            };

            map.MapElements.Add(icon);
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            map.MapElements.Clear();
            GetLocation();
        }

        private async void map_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon icon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;

            if (!string.IsNullOrEmpty(icon.Title))
            {
                try
                {
                    BasicGeoposition bgp = new BasicGeoposition()
                    {
                        Latitude = icon.Location.Position.Latitude,
                        Longitude = icon.Location.Position.Longitude
                    };

                    var center = new Geopoint(bgp);
                    await map.TrySetViewAsync(center, 16, 0, 0, MapAnimationKind.Bow); //set map center

                    Uri uri = new Uri("ms-drive-to:?destination.latitude=" + icon.Location.Position.Latitude.ToString().Replace(",", ".") + "&destination.longitude=" + icon.Location.Position.Longitude.ToString().Replace(",", ".") + "&destination.name=" + icon.Title);
                    await Windows.System.Launcher.LaunchUriAsync(uri);
                }

                catch (Exception)
                {

                }
            }
        }

        private void stationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationDetailsView.StationId = (stationListView.SelectedItem as StationModel).Id;
            StationDetailsView.StationName = (stationListView.SelectedItem as StationModel).Name;

            Frame.Navigate(typeof(StationDetailsView));
        }
    }
}