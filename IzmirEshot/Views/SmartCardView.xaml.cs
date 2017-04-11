using IzmirEshot.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Newtonsoft.Json;

namespace IzmirEshot.Views
{
    public sealed partial class SmartCardView : Page
    {
        private ApplicationDataContainer AppSettings;


        public SmartCardView()
        {
            this.InitializeComponent();
            this.Loaded += SmartCardView_Loaded;
            this.AppSettings = ApplicationData.Current.RoamingSettings;
        }


        private async Task<string> GetCardBalance(string cardId)
        {
            progress.IsIndeterminate = true;

            try
            {
                var balance = await App.Client.GetCardBalance(cardId);
                if (!string.IsNullOrEmpty(balance))
                {
                    progress.IsIndeterminate = false;
                    return balance;
                }

                else
                {
                    progress.IsIndeterminate = false;
                    return null;
                }
            }

            catch (Exception)
            {
                progress.IsIndeterminate = false;
                return null;
            }
        }

        private void SmartCardView_Loaded(object sender, RoutedEventArgs e)
        {
            object cards = AppSettings.Values["CardsFolder"];
            if (cards != null && cards.ToString() != "")
            {
                listView.ItemsSource = JsonConvert.DeserializeObject<List<SmartCardModel>>(cards as string);
            }
        }

        private async void enterButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (CardId.Text.Length == 11)
            {
                var balance = await GetCardBalance(CardId.Text);

                newCardNo.Text = CardId.Text;
                newCardBalance.Text = string.Concat("₺", balance);
                blurGrid.Visibility = Visibility.Visible;
                newBalanceGrid.Visibility = Visibility.Visible;
            }

            else
            {

            }
        }

        private void saveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            List<SmartCardModel> cardList = new List<SmartCardModel>();

            object cards = AppSettings.Values["CardsFolder"];
            if (cards == null)
            {
                var model = new SmartCardModel()
                {
                    Id = CardId.Text,
                    Name = (!string.IsNullOrEmpty(CardName.Text.Trim())) ? CardName.Text.Trim() : "Adsız",
                };

                cardList.Add(model);
                AppSettings.Values["CardsFolder"] = JsonConvert.SerializeObject(cardList);
            }

            else
            {
                if (JsonConvert.DeserializeObject<List<SmartCardModel>>(cards as string) == null)
                {
                    var model = new SmartCardModel()
                    {
                        Id = CardId.Text,
                        Name = (!string.IsNullOrEmpty(CardName.Text.Trim())) ? CardName.Text.Trim() : "Adsız",
                    };

                    cardList.Add(model);
                }

                else
                {
                    cardList = JsonConvert.DeserializeObject<List<SmartCardModel>>(cards as string);
                    var model = new SmartCardModel()
                    {
                        Id = CardId.Text,
                        Name = (!string.IsNullOrEmpty(CardName.Text.Trim())) ? CardName.Text.Trim() : "Adsız",
                    };

                    cardList.Add(model);
                }

                AppSettings.Values["CardsFolder"] = JsonConvert.SerializeObject(cardList);
            }

            listView.ItemsSource = cardList;
            oldBalanceGrid.Visibility = Visibility.Collapsed;
            newBalanceGrid.Visibility = Visibility.Collapsed;
            blurGrid.Visibility = Visibility.Collapsed;
        }

        private void deleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            object cards = AppSettings.Values["CardsFolder"];

            var cardList = JsonConvert.DeserializeObject<List<SmartCardModel>>(cards.ToString());

            foreach (var card in cardList)
            {
                var model = listView.SelectedItem as SmartCardModel;
                if (model.Id == card.Id && model.Name == card.Name)
                {
                    cardList.Remove(card);
                    break;
                }
            }

            AppSettings.Values["CardsFolder"] = JsonConvert.SerializeObject(cardList);

            listView.SelectedItem = null;
            listView.ItemsSource = cardList;
            
            oldBalanceGrid.Visibility = Visibility.Collapsed;
            newBalanceGrid.Visibility = Visibility.Collapsed;
            blurGrid.Visibility = Visibility.Collapsed;
        }

        private void closeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            oldBalanceGrid.Visibility = Visibility.Collapsed;
            newBalanceGrid.Visibility = Visibility.Collapsed;
            blurGrid.Visibility = Visibility.Collapsed;

            listView.SelectedItem = null;
        }

        private async void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCard = listView.SelectedItem as SmartCardModel;
            if (selectedCard != null)
            {
                var balance = await GetCardBalance(selectedCard.Id);

                oldCardNo.Text = selectedCard.Id;
                oldCardBalance.Text = string.Concat("₺", balance);
                blurGrid.Visibility = Visibility.Visible;
                oldBalanceGrid.Visibility = Visibility.Visible;
            }
        }
    }
}