using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.Interfaces;
using JukeBox.ViewModels;
using JukeBox.Models;
using System.Collections.ObjectModel;

namespace JukeBox.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePlaylistPopup : PopupPage
    {
        private static INavigation _nav;
        Song _song;
        public CreatePlaylistPopup( Song song)
        {
            InitializeComponent();
            _nav = Navigation;
            _song = song;
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

            if (MainViewModel.GetInstance().PlaylistViewModel.JukeBoxPlaylist.Where(r => r.Title == title).Count() > 0)
            {
                int i = 1;
                while (MainViewModel.GetInstance().PlaylistViewModel.JukeBoxPlaylist.Where(q => q.Title == $"{title}{i}").Count() > 0)
                {
                    i++;
                }
                title = $"{title}{i}";
            }

            DependencyService.Get<IPlaylistManager>().CreatePlaylist(title , _song);
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
