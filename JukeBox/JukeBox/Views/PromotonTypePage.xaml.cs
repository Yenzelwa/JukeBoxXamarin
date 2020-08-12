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

           // PromoListView.ItemsSource = suggestion;
        }
        private async void PromoListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (((ListView)sender).SelectedItem == null)
                return;

            var selectedItem = e.SelectedItem as PromotionType;
            if (selectedItem != null)
            {
               

               await Navigation.PushAsync(new PromoPage(selectedItem));
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
            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PromoTypeListView.IsVisible = true;
           // PromoListView.IsVisible = false;
        }
    }
}

    




