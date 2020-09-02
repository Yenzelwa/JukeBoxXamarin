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
    public partial class SongOptionsPopup : PopupPage
    {
        private static INavigation _nav;
        private Song _song;
        private int _isPlayListNameId;
        public  SongOptionsPopup(Song song , int playlistNameId)
        {
            if (song == null)
            {
                _isPlayListNameId = playlistNameId;
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
        private  void PlayNext(object sender, EventArgs e)
        {
              DependencyService.Get<IMusicManager>().PlayNext(new Song(_song as Song));
              Navigation.PopPopupAsync(true);
        }

        private  void AddToQueue(object sender, EventArgs e)
        {
            ObservableCollection<Song> songs = new ObservableCollection<Song>();
            songs.Add(new Song(_song as Song));
             DependencyService.Get<IMusicManager>().AddToEndOfQueue(songs);
            Navigation.PopPopupAsync(true);
        }
        private void AddToPlaylist(object sender, EventArgs e)
        {
            var song = _song;
            Navigation.PopAllPopupAsync(true);
            SongsPage.Nav?.PushPopupAsync(new CreatePlaylistPopup(_song),true);

        }
        private void DeleteSong(object sender, EventArgs e)
        {
            var dataService = new DataService();
            var mainViewModel = MainViewModel.GetInstance();
            var song = _song;

            if (mainViewModel.deletePlaylist)
            {
                var playlistByName = dataService.GetPlaylistById(song.Id, _isPlayListNameId);
                if(playlistByName !=null) dataService.Delete(playlistByName);
            }
            else
            {
                var audioFile = dataService.GetFileById(song.Id);
                if (audioFile != null)
                {
                    var playlist = dataService.GetPlaylistBySongId(song.Id);
                    if (playlist != null)
                    {
                        foreach (var play in playlist)
                        {
                            dataService.Delete(play);
                        }
                    }
                    dataService.Delete(audioFile);
                }
            }
                
                mainViewModel.PlaylistViewModel.Songs.Remove(song);
                Navigation.PopPopupAsync(true);
            //mainViewModel.PlaylistItems = new ObservableCollection<PlaylistItem>();
            //mainViewModel.PlaylistItems.Add(new PlaylistItem(
            //new Playlist { Title = "Home", IsDynamic = false }));
            ////  var file = DencryptFile(title + ".mp3", "");
            //mainViewModel.PlaylistViewModel = new PlaylistViewModel(mainViewModel.PlaylistItems[0]);
        }
           
        }
    }

