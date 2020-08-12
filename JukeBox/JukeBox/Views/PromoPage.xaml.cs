using FFImageLoading.Forms;
using JukeBox.BLL.Library;
using JukeBox.Helpers;
using JukeBox.Models;
using JukeBox.Services;
using JukeBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromoPage : ContentPage
    {
        private int promotionCategoryId = 0;
        public MainViewModel Main
        {
            get;
            set;
        }
        public PromoPage(PromotionType promotionType)
        {
            InitializeComponent();
            var mainViewModel = MainViewModel.GetInstance();
            if (promotionType.HasCategory == true)
            {
                mainViewModel.LibraryPromoModel.GetPromotionCategory(promotionType.PromotionTypeId);
                PromoCategoryListView.IsVisible = true;
            }
            else
            {


                ImgPromo.Source = promotionType.PromotionImage;
                mainViewModel.LibraryPromoModel.GetPromotionResult(promotionType.PromotionTypeId, 0);
                PromoCategoryListView.IsVisible = false;
                PromoListView.IsVisible = true;
            }
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = sender as SearchBar;
            var suggestion = MainViewModel.GetInstance().LibraryPromoModel.PromotionResult.Where(c => c.ArtistName.ToLower().Contains(search.Text.ToLower()));

            PromoListView.ItemsSource = suggestion;
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
        private async void PromoCategoryListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (((ListView)sender).SelectedItem == null)
                return;

            var selectedItem = e.SelectedItem as PromotionCategory;
            if (selectedItem != null)
            {
                var mainViewModel = MainViewModel.GetInstance();
                ImgPromo.Source = ImageSource.FromUri(new Uri(selectedItem.CategoryImage));
                mainViewModel.LibraryPromoModel.GetPromotionResult(selectedItem.PromotionTypeId, selectedItem.PromotionCategoryId);
                promotionCategoryId = selectedItem.PromotionCategoryId;
                PromoCategoryListView.IsVisible = false;
                PromoListView.IsVisible = true;

                //  await Navigation.PushAsync(new PromoPage(selectedItem));
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
                        mainViewModel.LibraryPromoModel.GetPromotionResult(promotion.PromotionTypeId ?? 0, promotionCategoryId);
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
    }
}