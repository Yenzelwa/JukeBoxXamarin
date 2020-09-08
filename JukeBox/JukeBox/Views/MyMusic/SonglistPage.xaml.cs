using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.Models;
using JukeBox.ViewModels;
using Rg.Plugins.Popup.Extensions;
using JukeBox.Controls;
using JukeBox.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JukeBox.Views.MyMusic
{
    [DesignTimeVisible(false)]
    public partial class SonglistPage : ContentPage
    {
        private static INavigation _nav;
        public SonglistPage(Albumlist albumlist)
        {

            InitializeComponent();
           // this.BindingContext = MusicStateViewModel.Instance;
           // Carousel.Position = MusicStateViewModel.Instance.QueuePos;
            if (albumlist != null && albumlist.AlbumsSongs.Count > 0)
            {
                Title.Text = albumlist.Name;
                Artist.Text =  albumlist.AlbumsSongs[0].Artist;
                img.Source = albumlist.ImageSource;
            }
             
            _nav = Navigation;
        }
        private void OpenNowPlayingPopup(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new NowPlayingPopup());
        }
        private void SongsPlaylistListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var _vm = MainViewModel.GetInstance().PlaylistViewModel;
            _vm.PlayCommand.Execute(e.Item);
            SongslistListView.SelectedItem = null;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(QueuePopup.Instance);
        }

        public static INavigation Nav
        {
            get
            {
                return _nav;
            }
        }
        protected async override void OnDisappearing()
        {
            var main = MainViewModel.GetInstance();
            if (main.PlaylistViewModel != null)
            {
                main.PlaylistViewModel.Songs = null;
                main.PlaylistViewModel.Songs = await DependencyService.Get<IPlaylistManager>().GetAllSongs();
                main.PlaylistViewModel.ShowArtwork = true;
            }

        }

    }
}
