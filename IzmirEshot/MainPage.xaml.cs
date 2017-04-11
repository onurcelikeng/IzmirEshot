using IzmirEshot.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace IzmirEshot
{
    public sealed partial class MainPage : Page
    {
        private int index;


        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeUI();
        }


        private void InitializeUI()
        {
            index = 0;
            panel1.Opacity = 1;
            panel2.Opacity = 0;
            panel3.Opacity = 0;
            panel4.Opacity = 0;
            App.Client.deneme();
            GetSchedules();
            iframe.Navigate(typeof(SearchView));
        }

        private async void GetSchedules()
        {
            try
            {
                BusDetailsView.scheduleList = await App.Client.GetSchedules();
            }

            catch (System.Exception)
            {

            }
        }

        private void searchButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (index != 0)
            {
                index = 0;
                panel1.Opacity = 1;
                panel2.Opacity = 0;
                panel3.Opacity = 0;
                panel4.Opacity = 0;

                iframe.Navigate(typeof(SearchView));
            }
        }

        private void smartCardButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (index != 1)
            {
                index = 1;
                panel1.Opacity = 0;
                panel2.Opacity = 1;
                panel3.Opacity = 0;
                panel4.Opacity = 0;

                iframe.Navigate(typeof(SmartCardView));
            }
        }

        private void closePlaceButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (index != 2)
            {
                index = 2;
                panel1.Opacity = 0;
                panel2.Opacity = 0;
                panel3.Opacity = 1;
                panel4.Opacity = 0;

                iframe.Navigate(typeof(NearPlacesView));
            }
        }

        private void moreButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (index != 3)
            {
                index = 3;
                panel1.Opacity = 0;
                panel2.Opacity = 0;
                panel3.Opacity = 0;
                panel4.Opacity = 1;

                iframe.Navigate(typeof(MoreView));
            }
        }
    }
}