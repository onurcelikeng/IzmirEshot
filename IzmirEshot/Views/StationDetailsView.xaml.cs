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
    public sealed partial class StationDetailsView : Page
    {
        public static string StationName;
        public static int StationId;


        public StationDetailsView()
        {
            this.InitializeComponent();
            this.Loaded += StationDetailsView_Loaded;
        }


        private async void DuraktanGecenBuses()
        {
            progress2.IsIndeterminate = true;

            try
            {
                var response = await App.Client.SearchBusStops(StationId.ToString());

                var split = response[0].Bus.Split(';');
                List<BusModel> list = new List<BusModel>();

                for (int i = 0; i < SearchView.busList.Count; i++)
                {
                    for (int j = 0; j < split.Length; j++)
                    {
                        if (SearchView.busList[i].Id.ToString() == split[j])
                        {
                            list.Add(SearchView.busList[i]);
                        }
                    }
                }

                listView2.ItemsSource = list;

            }

            catch (Exception)
            {

            }

            progress2.IsIndeterminate = false;
        }

        private async void GetOncomingBusesFromStation()
        {
            progress1.IsIndeterminate = true;

            try
            {
                var response = await App.Client.GetOncomingBusesToStation(StationId);
                listView1.ItemsSource = response;
            }

            catch (Exception)
            {

            }

            progress1.IsIndeterminate = false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            GetOncomingBusesFromStation();
            DuraktanGecenBuses();
        }

        private void StationDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            Header.Text = StationName;
            GetOncomingBusesFromStation();
            DuraktanGecenBuses();
        }
    }
}