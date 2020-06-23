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

namespace JukeBox.Views.MyMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongsPage : ContentPage
    {
        private static INavigation _nav;
        public SongsPage()
        {
          // _vm = new PlaylistViewModel(playlistItem);
          //  this.BindingContext = _vm;
            InitializeComponent();
            _nav = Navigation;
        }

        private void SongslistListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var _vm = MainViewModel.GetInstance().PlaylistViewModel;
            _vm.PlayCommand.Execute(e.Item);
            SongslistListView.SelectedItem = null;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(QueuePopup.Instance);
        }
        private async void TapDetail_OnTapped(object sender, EventArgs e)
        {
            var img = ((Image)sender);
            Albumlist album = img.BindingContext as Albumlist;
            var main = MainViewModel.GetInstance();
            main.PlaylistViewModel.Songs = album.AlbumsSongs;
           // await DependencyService.Get<IMusicManager>().SetQueue(album.Songs);
            await Navigation.PushAsync(new SonglistPage(album));
        }
        private async void TapPlayDetail_OnTapped(object sender, EventArgs e)
        {
            var img = ((Image)sender);
            PlaylistItem playlistItem = img.BindingContext as PlaylistItem;
           var songsPlaylist =  await DependencyService.Get<IPlaylistManager>().GetPlaylistSongs(playlistItem.Playlist.Id);
            var main = MainViewModel.GetInstance();
            main.PlaylistViewModel.Songs = songsPlaylist;
            var album = new Albumlist();
            album.Name = playlistItem.Playlist.Title;
            album.AlbumsSongs = songsPlaylist;
            // await DependencyService.Get<IMusicManager>().SetQueue(album.Songs);
            await Navigation.PushAsync(new SonglistPage(album));
        }

        public static INavigation Nav
        {
            get
            {
                return _nav;
            }
        }

        private async void Button_Album(object sender, EventArgs e)
        {
            Albumlist album = this.BindingContext as Albumlist;
            //SongslistListView.IsVisible = false;
            //playlistListView.IsVisible = false;
            //albumListView.IsVisible = true;
            ////await DependencyService.Get<IMusicManager>().SetQueue(new ObservableCollection<Song>(
            //    vm.Songs.Select(s => new Models.Song(s))));
            //DependencyService.Get<IMusicManager>().Shuffle();
        }

        private async void Button_Songs(object sender, EventArgs e)
        {
            MainViewModel vm = this.BindingContext as MainViewModel;
            var main = MainViewModel.GetInstance();
            if (main.PlaylistViewModel != null)
            {
                main.PlaylistViewModel.Songs = await DependencyService.Get<IPlaylistManager>().GetAllSongs();
            }
                //// await DependencyService.Get<IMusicManager>().SetQueue(main.PlaylistViewModel.Songs);
                //SongslistListView.IsVisible = true;
                //playlistListView.IsVisible = false;
                //albumListView.IsVisible = false;
            
            //DependencyService.Get<IMusicManager>().AddToEndOfQueue(new ObservableCollection<Song>(
            //    vm.PlaylistViewModel.Songs.Select(s => new Models.Song(s))));
        }
        private async void Button_Playlist(object sender, EventArgs e)
        { 
        //{
        //    SongslistListView.IsVisible = false;
        //    playlistListView.IsVisible = true;
        //    albumListView.IsVisible = false;
           // var main = MainViewModel.GetInstance();

           //var playlists = await DependencyService.Get<IPlaylistManager>().GetPlaylists();
           // foreach (var item in playlists)
           // {
           //     main.PlaylistItems.Add(new PlaylistItem(item));
           // }
        }
    }
}
