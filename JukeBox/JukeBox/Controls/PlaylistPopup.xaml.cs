using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.Interfaces;
using JukeBox.Models;
using JukeBox.Services;
using JukeBox.ViewModels;
using JukeBox.Views;
using JukeBox.Views.MyMusic;

namespace JukeBox.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistPopup : PopupPage
    {
        private static INavigation _nav;
        private Song _song;
        public PlaylistPopup(Song song)
        {
            if (song == null)
            {
                return;
            }
            _song = song;

            this.BindingContext = MusicStateViewModel.Instance;
            InitializeComponent();
            _nav = Navigation;

        }
        public static INavigation Nav
        {
            get
            {
                return _nav;
            }
        }
        private async void AddToPlaylist(object sender, EventArgs e)
        {

            var btn = ((Button)sender);
            var playlistItem = btn.BindingContext as JukeBoxPlaylist;
            DependencyService.Get<IPlaylistManager>().AddToPlaylist(
                           playlistItem,
                           _song);
            var playlists = await DependencyService.Get<IPlaylistManager>().GetPlaylists();
            if (playlists.Count > 0)
            {
                var main = MainViewModel.GetInstance();
                main.PlaylistViewModel.JukeBoxPlaylist = playlists;
            }
            await Navigation.PopPopupAsync(true);

        }
        private async void CreatePlaylist(object sender, EventArgs e)
        {
            string title;
            if (String.IsNullOrWhiteSpace(PlaylistNameEntry.Text))
            {
                title = "Untitled Playlist";
            }
            else
            {
                title = PlaylistNameEntry.Text;
            }

            //if (MenuViewModel.Instance.PlaylistItems.Where(r => r.Playlist?.Title == title).Count() > 0)
            //{
            //    int i = 1;
            //    while (MenuViewModel.Instance.PlaylistItems.Where(q => q.Playlist?.Title == $"{title}{i}").Count() > 0)
            //    +
            //        i++;
            //    }
            //    title = $"{title}{i}";
            //}

            DependencyService.Get<IPlaylistManager>().CreatePlaylist(title, _song);
            MenuViewModel.Instance.Refresh();
            await Navigation.PopPopupAsync(true);
            var playlists = await DependencyService.Get<IPlaylistManager>().GetPlaylists();
            if (playlists.Count > 0)
            {
                var main = MainViewModel.GetInstance();
                main.PlaylistViewModel.JukeBoxPlaylist = playlists;
            }

        }
    }
}