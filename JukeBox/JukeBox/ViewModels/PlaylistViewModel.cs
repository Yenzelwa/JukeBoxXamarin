using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using JukeBox.Helpers;
using JukeBox.Interfaces;
using JukeBox.Models;
using JukeBox.Views;
using JukeBox.Views.MyMusic;
using Android.Widget;

namespace JukeBox.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public static PlaylistViewModel Instance { get; set; }
        public MusicStateViewModel MusicState { get { return MusicStateViewModel.Instance; } }

        public Command PlayCommand { get; set; }
        public Command AlbumQueueCommand { get; set; }
        public Command AlbumSongCommand { get; set; }
        public Command AddSong { get; set; }
        public Command PlaylistSongCommand { get; set; }
        public Command ShuffleCommand { get; set; }


        public ulong Id { get; set; }
        public long AlbumId { get; set; }
        public bool deletePlaylist { get; set; }

        private SongComparer _comparer;
        private Playlist _playlist;
        private IList<Albumlist> _albumlist;
        private IList<JukeBoxPlaylist> _jukeBoxPlaylist;

        public PlaylistViewModel(PlaylistItem playlistItem)
        {
            Instance = this;
            _comparer = new SongComparer();
            _playlist = playlistItem.Playlist;
            Id = _playlist.Id;

            Title = playlistItem.Playlist.Title;
           
                Task.Run(async () =>
                {

                    SongsLoading = true;
                    OnPropertyChanged(nameof(SongsLoading));
                    
                    Album = await DependencyService.Get<IPlaylistManager>().GetSongsByAlbum();
                    JukeBoxPlaylist = await DependencyService.Get<IPlaylistManager>().GetPlaylists();
                    Songs = await DependencyService.Get<IPlaylistManager>().GetAllSongs();
                       
                    await DependencyService.Get<IMusicManager>().SetQueue(Songs);

                    SongsLoading = false;
                    OnPropertyChanged(nameof(SongsLoading));
                    //if (playlistItem.Playlist !=null && _playlist.Id > 0)                  
                    //{
                        
                     //  Songs = await DependencyService.Get<IPlaylistManager>().GetPlaylistSongs(playlistItem.Playlist.Id);
                    //    await DependencyService.Get<IMusicManager>().SetQueue(Songs);
                    //}
                    
                    SongsLoading = false;
                    OnPropertyChanged(nameof(SongsLoading));
              
          
          });
            PlaylistSongCommand = new Command((item) =>
            {
                var jukeBoxPlaylist = item as JukeBoxPlaylist;
                if (jukeBoxPlaylist != null)
                {

                    var playlistViewModel = MainViewModel.GetInstance().PlaylistViewModel;

                    playlistViewModel.PlaylistSongs = jukeBoxPlaylist.PlaylistSongs;
                    (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new PlaylistsSongPage(jukeBoxPlaylist));
                    //  await Application.Current.MainPage.Navigation.PushAsync(new SonglistPage(albumItem));

                }


            });
            AddSong = new Command(() =>
            {
                Task.Run(async () =>
                {
                    //DependencyService.Get<IPlaylistManager>().AddToPlaylist(
                    //   Playlist,
                    //   MusicStateViewModel.Instance.SelectedSong);
                    //  playlist.Songs = await DependencyService.Get<IPlaylistManager>().GetPlaylistSongs(
                    //   playlist.Id);
                    //  await  (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PopPopupAsync(true);

                   
                });


            });
            PlayCommand = new Command((item) =>
            {
              var  songItem = item as Song;
                var song = Songs.Where(x => x.Id == songItem.Id).FirstOrDefault();
                if (song == null) Songs.Add(songItem);   
                var index = Songs.IndexOf(song);
                this.MusicState.QueuePos = index;

                DependencyService.Get<IMusicManager>().StartQueue(new ObservableCollection<Song>(Songs), index);
            });

            AlbumQueueCommand = new Command((item) =>
            {
               if(this.Songs != null)
                {
                    DependencyService.Get<IMusicManager>().StartQueue(new ObservableCollection<Song>(Songs), 0);
                }
               
            });

            AlbumSongCommand = new Command( (item) =>
            {
                var albumItem = item as Albumlist;  
                if(albumItem != null)
                {
                    var playlistViewModel = MainViewModel.GetInstance().PlaylistViewModel;
                    playlistViewModel.AlbumSongs = albumItem.AlbumsSongs;
                    ShowArtwork = false;
                    (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new SonglistPage(albumItem));
                  //  await Application.Current.MainPage.Navigation.PushAsync(new SonglistPage(albumItem));
                }
                
            
            });
     
            ShuffleCommand = new Command(() => DependencyService.Get<IMusicManager>().Shuffle());
            if (Songs !=null)
            {
                HasSongs = true;
            }
            else
            {
                HasSongs = false;
            }


        }

        public string Title { get; set; }

        private IList<Song> _songs;
        private IList<Song> _albumSongs;
        private IList<Song> _playlistSongs;
        public IList<Song> Songs
        {
            get { return _songs; }
            set
            {
                _songs = value;
                //   _playlist.Songs = _songs;
                OnPropertyChanged(nameof(Songs));
                OnPropertyChanged(nameof(CountText));
                OnPropertyChanged(nameof(HasSongs));
            }
        }
        public IList<Song> AlbumSongs
        {
            get { return _songs; }
            set
            {
                _songs = value;

                //   _playlist.Songs = _songs;
                OnPropertyChanged(nameof(Album));
                OnPropertyChanged(nameof(CountText));
                OnPropertyChanged(nameof(HasSongs));
            }
        }
        public IList<Song> PlaylistSongs
        {
            get { return _songs; }
            set
            {
                _songs = value;

                //   _playlist.Songs = _songs;
               
                OnPropertyChanged(nameof(JukeBoxPlaylist));
                OnPropertyChanged(nameof(CountText));
                OnPropertyChanged(nameof(HasSongs));
            }
        }
        public IList<Albumlist> Album
        {
            get { return _albumlist; }
            set
            {
                _albumlist = value;
                _playlist.Songs = _songs;
                OnPropertyChanged(nameof(Songs));
                OnPropertyChanged(nameof(Album));
                OnPropertyChanged(nameof(CountText));
                OnPropertyChanged(nameof(HasSongs));
                OnPropertyChanged(nameof(ShowArtwork));
            }
        }
        public IList<JukeBoxPlaylist> JukeBoxPlaylist
        {
            get { return _jukeBoxPlaylist; }
            set
            {
                _jukeBoxPlaylist = value;
                _playlist.Songs = _songs;
                OnPropertyChanged(nameof(Songs));
                OnPropertyChanged(nameof(JukeBoxPlaylist));
                OnPropertyChanged(nameof(CountText));
                OnPropertyChanged(nameof(HasSongs));
                OnPropertyChanged(nameof(ShowArtwork));
            }
        }

        public bool HasSongs { get; set; }
      

        public string CountText
        {
            get
            {
                return Songs != null ? $"{Songs.Count} Songs" : "0 Songs";
            }
        }

        public bool SongsLoading { get; set; }
        public bool Repeat { get; set; }
        public bool Shuffle { get; set; }
        public bool ShowArtwork { get; set; }

    }
}
