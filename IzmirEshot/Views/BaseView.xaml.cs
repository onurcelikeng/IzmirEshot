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
    public sealed partial class BaseView : Page
    {
        private List<BaseModel> list;
        public static string Title;
                

        public BaseView()
        {
            this.InitializeComponent();
            this.Header.Text = Title;
            this.Loaded += BaseView_Loaded;
        }


        private async void GetMetroStations()
        {
            progress.IsIndeterminate = true;

            try
            {
                list = await App.Client.GetMetroStations();
                listView.ItemsSource = list;
                if(list.Count != 0) line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private async void GetIzbanStations()
        {
            progress.IsIndeterminate = true;

            try
            {
                list = await App.Client.GetIzbanStations();
                listView.ItemsSource = list;
                if (list.Count != 0) line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private async void GetVapurStations()
        {
            progress.IsIndeterminate = true;

            try
            {
                list = await App.Client.GetVapurStations();
                listView.ItemsSource = list;
                if (list.Count != 0) line.Opacity = 1;
            }

            catch (Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private void BaseView_Loaded(object sender, RoutedEventArgs e)
        {
            if(Title == "Metro Bağlantılı Hatlar")
            {
                GetMetroStations();
            }

            else if (Title == "İZBAN Bağlantılı Hatlar")
            {
                GetIzbanStations();
            }

            else if (Title == "Vapur Bağlantılı Hatlar")
            {
                GetVapurStations();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBusView.Title = (listView.SelectedItem as BaseModel).Name;
            Frame.Navigate(typeof(SearchBusView));
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string filter = sender.Text.ToUpper();
            listView.ItemsSource = list.Where(s => (s.Name).ToUpper().Contains(filter));

        }
    }
}