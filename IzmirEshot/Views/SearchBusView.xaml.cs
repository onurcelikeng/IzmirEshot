using IzmirEshot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IzmirEshot.Views
{
    public sealed partial class SearchBusView : Page
    {
        private List<BusModel> busList;
        public static string Title;


        public SearchBusView()
        {
            this.InitializeComponent();
            this.Loaded += SearchBusView_Loaded;
        }


        private async void SearchBuses()
        {
            progress.IsIndeterminate = true;

            try
            {
                busList = await App.Client.SearchBuses(Title);
                busListView.ItemsSource = busList;
                if (busList.Count != 0) line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void SearchBusView_Loaded(object sender, RoutedEventArgs e)
        {
            Header.Text = Title;
            SearchBuses();
        }

        private void busListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusDetailsView.selectedBus = busListView.SelectedItem as BusModel;
            Frame.Navigate(typeof(BusDetailsView));
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string filter = sender.Text.ToUpper();

            busListView.ItemsSource = busList.Where(s => (s.Id + s.Name).ToUpper().Contains(filter));
        }
    }
}