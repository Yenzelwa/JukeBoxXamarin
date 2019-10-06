
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.Controller;
using JukeBox.Models;
using JukeBox.ViewModels;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {
        public static bool IsUWPDesktop { get; set; }
        public RootPage()
        {
            InitializeComponent();
            if (IsUWPDesktop)
              this.MasterBehavior = MasterBehavior.Popover;
           App.Navigator = Navigator;
            App.Master = this;
           // Master = new MenuPage();
            

            //setup home page
            // Pages.Add((int)MenuType.Browse, new MusicAppNavigationPage(new HomeTabbedPage()));
            // Detail = Pages[(int)MenuType.Browse];

            //  InvalidateMeasure();
         var   PlaylistItems = new ObservableCollection<PlaylistItem>();
            PlaylistItems.Add(new PlaylistItem(
            new Playlist { Title = "Home", IsDynamic = false }));
            LandingTabbedPage page = new LandingTabbedPage(PlaylistItems[0]);
            Detail = new NavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex("#2f3037"),
                BarTextColor = Color.White,
                Title = "Home"
            };
            // InitializeComponent();

            //MenuPage.PlaylistList.ItemSelected += (s, e) =>
            //{
            //    PlaylistItem item = e.SelectedItem as PlaylistItem;
            //    if (item != null)
            //    {
            //        // To display the Home page since it isn't a PlaylistPage
            //        if (item.Playlist.IsDynamic == false && item.Playlist.Title == "Home")
            //        {
            //            LandingTabbedPage page = new LandingTabbedPage(item);
            //            Detail = new NavigationPage(page)
            //            {
            //                BarBackgroundColor = Color.DarkGreen,
            //                BarTextColor = Color.White
            //            };
            //        }
            //        else  
            //        {

            //            PlaylistViewModel.Instance = null;
            //            Detail = new NavigationPage(new HomePage(this))
            //            {
            //                BarBackgroundColor = Color.DarkGreen,
            //                BarTextColor = Color.White
            //            };
            //        }
             


            //        IsPresented = false;
            //    }
            //};

           // UpdateSelected(MenuViewModel.Instance.PlaylistItems.First());
        }

        public void UpdateSelected(object item)
        {
            if (item != null)
            {
               // MenuPage.PlaylistList.SelectedItem = item;

            }
        }
        public async Task NavigateAsync(int id)
        {

            if (Detail != null)
            {
                if (IsUWPDesktop || Xamarin.Forms.Device.Idiom != TargetIdiom.Tablet)
                    IsPresented = false;

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    await Task.Delay(300);
            }

            Page newPage;
            //if (!Pages.ContainsKey(id))
            //{

            //    switch (id)
            //    {
            //        case (int)MenuType.Browse:
            //            Pages.Add(id, new MusicAppNavigationPage(new HomeTabbedPage()));
            //            break;
           

            //    }
            //}

          //  newPage = Pages[id];
         //   if (newPage == null)
         //       return;

            //pop to root for Windows Phone
            //if (Detail != null && Device.RuntimePlatform == Device.macOS)
            //{
            //    await Detail.Navigation.PopToRootAsync();
            //}

          //  Detail = newPage;

        }
    }
}
