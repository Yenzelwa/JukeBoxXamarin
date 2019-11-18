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

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeaturedPage : ContentPage
    {
        public MainViewModel Main
        {
            get;
            set;
        }
        public FeaturedPage()
        {
            InitializeComponent();
            GetMovies();
            


        }

        public async void GetMovies()
        {

            SLMovies.IsVisible = false;

            try
            {
              //  SLLoader.IsVisible = true;
                SLMovies.IsVisible = true;
                var test = MainViewModel.GetInstance();

            }
            catch (Exception e)
            {
              // SLLoader.IsVisible = false;
                throw;
            }
            finally
            {
              // SLLoader.IsVisible = false;
            }

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
     
        
    }
}

    




