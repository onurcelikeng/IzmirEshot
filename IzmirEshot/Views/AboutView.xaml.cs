using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IzmirEshot.Views
{
    public sealed partial class AboutView : Page
    {

        public AboutView()
        {
            this.InitializeComponent();
            this.Loaded += AboutView_Loaded;
        }


        private async void AboutView_Loaded(object sender, RoutedEventArgs e)
        {
            progress.IsIndeterminate = true;

            try
            {
                string content = await App.Client.GetAbout();

                if (content != null)
                {
                    webView.NavigateToString(content);
                }
            }

            catch (Exception) { }

            progress.IsIndeterminate = false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}