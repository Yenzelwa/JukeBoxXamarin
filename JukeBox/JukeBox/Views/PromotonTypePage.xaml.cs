using System;
using JukeBox.Models;
using JukeBox.ViewModels;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using JukeBox.BLL.Library;
using System.Collections.ObjectModel;
using JukeBox.Interfaces;
using System.Linq;
using DLToolkit.Forms.Controls;
using Xamarin.Forms.Xaml;
using JukeBox.Infrastructure;
using FFImageLoading.Forms;
using JukeBox.Helpers;
using JukeBox.Services;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromotionTypePage : ContentPage
    {
        public MainViewModel Main
        {
            get;
            set;
        }
        public PromotionTypePage()
        {
            InitializeComponent();


        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = sender as SearchBar;
            var suggestion = MainViewModel.GetInstance().LibraryPromoModel.PromotionResult.Where(c => c.ArtistName.ToLower().Contains(search.Text.ToLower()));

            PromoListView.ItemsSource = suggestion;
        }
        private async void PromoListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (((ListView)sender).SelectedItem == null)
                return;

            var selectedItem = e.SelectedItem as PromotionType;
            if (selectedItem != null)
            {
                var mainViewModel = MainViewModel.GetInstance();
               ImgPromo.Source = ImageSource.FromUri(new Uri(selectedItem.PromotionImage));
                mainViewModel.LibraryPromoModel.GetPromotionResult(selectedItem.PromotionTypeId);
                PromoTypeListView.IsVisible = false;
                PromoListView.IsVisible = true;

              //  await Navigation.PushAsync(new PromoPage(selectedItem));
                ((ListView)sender).SelectedItem = null;
            }
              ((ListView)sender).SelectedItem = null;

        }
        private async void MovieListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (((ListView)sender).SelectedItem == null)
                return;

            var selectedItem = e.SelectedItem as ApiLibrary;
            if (selectedItem != null)
            {

                await Navigation.PushAsync(new MusicDetailPage(selectedItem));
                ((ListView)sender).SelectedItem = null;
            }
          ((ListView)sender).SelectedItem = null;

        }

        private async void TapGestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            var img = ((CachedImage)sender);
            var mainViewModel = MainViewModel.GetInstance();
            var apiService = new ApiService();
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            if (img.BindingContext is PromotionResult promotion)
            {
                var checkConnetion = await apiService.CheckConnection();
                if (!checkConnetion.IsSuccess)
                {
                    this.IsEnabled = true;
                    await DisplayAlert(
                        Languages.Error,
                        checkConnetion.Message,
                        Languages.Accept);
                    return;
                }
                var request = new PromotionResultRequest
                {
                    Customer = mainViewModel.User.UserId,
                    ClientId = promotion.ArtistId,
                    PromotionTypeId = promotion.PromotionTypeId ?? 0
                };
                var voteResponse = await Library.Vote(request);
                if (voteResponse != null)
                {
                    if (voteResponse.ResponseMessage != "Success")
                    {
                        await DisplayAlert(
                       Languages.Error,
                       voteResponse.ResponseMessage,
                       Languages.Accept);
                        return;
                    }
                    else
                    {
                        mainViewModel.LibraryPromoModel.GetPromotionResult(promotion.PromotionTypeId ?? 0);
                        var user = await apiService.GetUserByEmail(
             apiSecurity,
             "/api/account",
             "/customer/getcustomer",
             mainViewModel.Token.TokenType,
             mainViewModel.Token.AccessToken,
             mainViewModel.Token.UserName);
                        mainViewModel.Login.registerDataService(user, mainViewModel.Token);
                        //mainViewModel.LibraryPromoModel.PromotionResult = 
                    }
                }
            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PromoTypeListView.IsVisible = true;
            PromoListView.IsVisible = false;
        }
    }
}

    




