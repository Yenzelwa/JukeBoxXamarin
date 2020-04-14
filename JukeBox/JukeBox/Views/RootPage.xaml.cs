
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
                BarBackgroundColor = Color.FromHex("#232323"),
                BarTextColor = Color.White,
                Title = "Home"
            };
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
          

        }
    }
}
