
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using JukeBox.Models;
using JukeBox.ViewModels;
using JukeBox.Views;

namespace JukeBox.Views
{
    public partial class LandingTabbedPage : TabbedPage
    {
        private PlaylistViewModel _vm;

        public LandingTabbedPage(PlaylistItem playlistItem)
        {
            InitializeComponent();
            this.Children.Add(
                new FeaturedPage()
                {
                    Title = "Store"
                    
                });
            this.Children.Add(
    new PlaylistPage(playlistItem)
    {
        Title = "My Music"
    });
            this.Children.Add(
  new PromoPage()
  {
      Title = "Promo"
  });

            this.Children.Add(
             new RechargePage()
             {
                 Title = "Recharge"
             });
            this.CurrentPageChanged += tabChanged;
        }
        protected void tabChanged(object sender, EventArgs args)
        { //The children are navigation pages so we can easily pop to root of previous selected page //if you have something else than NavigationPages make sure to get the good reference //Keep an integer as reference to the currently selected page index within your class //Pop to root for previous index 
          // ((NavigationPage)this.Children[selectedPageIndex]).PopToRootAsync(); //assign page to index
          //  this.selectedPageIndex = this.Children.IndexOf(this.CurrentPage); }
            var current = this.CurrentPage;
            var PlaylistItems = new ObservableCollection<PlaylistItem>();
            PlaylistItems.Add(new PlaylistItem(
            new Playlist { Title = "Home", IsDynamic = false }));
          //  var vUpdatedPage = new PlaylistPage(PlaylistItems[0]);
            this.CurrentPage.Navigation.PopToRootAsync();
           // Navigation.PopAsync();
        }
    }
}

