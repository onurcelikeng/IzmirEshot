using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace IzmirEshot.Views
{
    public sealed partial class MoreView : Page
    {

        public MoreView()
        {
            this.InitializeComponent();
        }


        private async void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = (sender as Grid).Name;

            if(button == "metroButton")
            {
                BaseView.Title = "Metro Bağlantılı Hatlar";
                Frame.Navigate(typeof(BaseView));
            }

            else if (button == "izbanButton")
            {
                BaseView.Title = "İZBAN Bağlantılı Hatlar";
                Frame.Navigate(typeof(BaseView));
            }

            else if (button == "vapurButton")
            {
                BaseView.Title = "Vapur Bağlantılı Hatlar";
                Frame.Navigate(typeof(BaseView));
            }

            else if (button == "otogarButton")
            {
                SearchBusView.Title = "Otogar Bağlantılı Hatlar";
                Frame.Navigate(typeof(SearchBusView));
            }

            else if (button == "havalimaniButton")
            {
                SearchBusView.Title = "Havalimanı Seferleri";
                Frame.Navigate(typeof(SearchBusView));
            }

            else if(button == "geribildirimButton")
            {
                await Launcher.LaunchUriAsync(new Uri("mailto:posta@onurcelikeng.com"));
            }

            else if (button == "hakkindaButton")
            {
                Frame.Navigate(typeof(AboutView));
            }

        }
    }
}