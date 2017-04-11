using IzmirEshot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IzmirEshot.Views
{
    public sealed partial class SearchView : Page
    {
        public static List<BusModel> busList;


        public SearchView()
        {
            this.InitializeComponent();
            this.Loaded += SearchView_Loaded;
        }


        private async void GetBuses()
        {
            progress.IsIndeterminate = true;

            try
            {
                busList = await App.Client.GetBuses();
                busListView.ItemsSource = busList;
                if (busList.Count != 0) line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private void SearchView_Loaded(object sender, RoutedEventArgs e)
        {
            if(busList == null)
            {
                GetBuses();
            }

            else
            {
                busListView.ItemsSource = busList;
                if (busList.Count != 0) line.Opacity = 1;
            }
        }

        private void busListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusDetailsView.selectedBus = busListView.SelectedItem as BusModel;
            Frame.Navigate(typeof(BusDetailsView));
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string filter = sender.Text.ToUpper();
            //if (filter.Length > 3)
            //    listView.ItemsSource = await App.Client.SearchBusStops(sender.Text);

            busListView.ItemsSource = busList.Where(s => (s.Id + s.Name).ToUpper().Contains(filter));
        }
    }
}