using IzmirEshot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IzmirEshot.Views
{
    public sealed partial class StationsView : Page
    {
        private int direction;


        public StationsView()
        {
            this.InitializeComponent();
            this.Loaded += StationsView_Loaded;
            this.DataContext = BusDetailsView.selectedBus;
            this.direction = 0;
        }


        private async void GetBusStops()
        {
            progress.IsIndeterminate = true;

            try
            {
                var stationList = await App.Client.GetBusStops(BusDetailsView.selectedBus.Id, direction);
                stationListView.ItemsSource = stationList;
                if (stationList.Count != 0) line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private void StationsView_Loaded(object sender, RoutedEventArgs e)
        {
            GetBusStops();
        }

        private void changeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(direction == 0)
            {
                direction = 1;
                StartStation.Text = BusDetailsView.selectedBus.FinishStation;
                FinishStation.Text = BusDetailsView.selectedBus.StartStation;
            }

            else if (direction == 1)
            {
                direction = 0;
                StartStation.Text = BusDetailsView.selectedBus.StartStation;
                FinishStation.Text = BusDetailsView.selectedBus.FinishStation;
            }

            GetBusStops();

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void stationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationDetailsView.StationId = (stationListView.SelectedItem as StationModel).Id;
            StationDetailsView.StationName = (stationListView.SelectedItem as StationModel).Name;

            Frame.Navigate(typeof(StationDetailsView));
        }
    }
}