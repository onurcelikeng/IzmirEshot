using IzmirEshot.Models;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace IzmirEshot.Views
{
    public sealed partial class BusDetailsView : Page
    {
        public static List<ScheduleModel> scheduleList;
        public static BusModel selectedBus;


        public BusDetailsView()
        {
            this.InitializeComponent();
            this.DataContext = selectedBus;
            this.Loaded += BusDetailsView_Loaded;
        }


        private async void GetWeekdayTimes()
        {
            progress.IsIndeterminate = true;

            try
            {
                weekdayList.ItemsSource = await App.Client.GetTransportationTime(selectedBus.Id, scheduleList[0].Id);
            }

            catch (System.Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private async void GetSaturdayTimes()
        {
            progress.IsIndeterminate = true;

            try
            {
                saturdayList.ItemsSource = await App.Client.GetTransportationTime(476, scheduleList[1].Id);
            }

            catch (System.Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private async void GetSundayTimes()
        {
            progress.IsIndeterminate = true;

            try
            {
                sundayList.ItemsSource = await App.Client.GetTransportationTime(476, scheduleList[2].Id);
            }

            catch (System.Exception)
            {

            }

            progress.IsIndeterminate = false;
        }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string buttonName = (sender as TextBlock).Name;

            if (buttonName == "weekdayButton") pivot.SelectedItem = pivotItem_1;
            else if (buttonName == "saturdayButton") pivot.SelectedItem = pivotItem_2;
            else if (buttonName == "sundayButton") pivot.SelectedItem = pivotItem_3;
        }

        private void BusDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            var dayOfWeek = DateTime.Now.DayOfWeek.ToString();

            if (dayOfWeek == "Saturday") pivot.SelectedItem = pivotItem_2;
            else if (dayOfWeek == "Sunday") pivot.SelectedItem = pivotItem_3;
            else pivot.SelectedItem = pivotItem_1;
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedItem == pivotItem_1)
            {
                weekdayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 220, 31, 51));
                saturdayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 130, 174));
                sundayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 130, 174));

                GetWeekdayTimes();
            }

            else if (pivot.SelectedItem == pivotItem_2)
            {
                weekdayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 130, 174));
                saturdayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 220, 31, 51));
                sundayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 130, 174));

                GetSaturdayTimes();
            }

            else if (pivot.SelectedItem == pivotItem_3)
            {
                weekdayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 130, 174));
                saturdayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 85, 130, 174));
                sundayButton.Foreground = new SolidColorBrush(Color.FromArgb(255, 220, 31, 51));

                GetSundayTimes();
            }
        }

        #region Navigation

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void stationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StationsView));
        }

        #endregion
    }
}