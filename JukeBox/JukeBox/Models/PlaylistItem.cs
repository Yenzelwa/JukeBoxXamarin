﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using JukeBox.Interfaces;
using JukeBox.ViewModels;
using JukeBox.Views.MyMusic;
using Rg.Plugins.Popup.Extensions;

namespace JukeBox.Models
{
    public class PlaylistItem : BaseViewModel
    {
        public Playlist Playlist { get; set; }

        public Command AddSong { get; set; }

        private bool _isToggled;
        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                _isToggled = value;
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public bool CanAdd
        {
            get
            {
                return Playlist.IsDynamic && _isToggled;
            }
        }


        public PlaylistItem(Playlist playlist)
        {
            Playlist = playlist;
            if (Playlist.IsDynamic || !playlist.IsDynamic)
            {
                //AddSong = new Command(() =>
                //{
                //    Task.Run(async () =>
                //    {
                //         DependencyService.Get<IPlaylistManager>().AddToPlaylist(
                //            Playlist,
                //            MusicStateViewModel.Instance.SelectedSong);
                //        //  playlist.Songs = await DependencyService.Get<IPlaylistManager>().GetPlaylistSongs(
                //        //   playlist.Id);
                //    //  await  (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PopPopupAsync(true);

                //        if (PlaylistViewModel.Instance?.Id == playlist.Id)
                //        {
                //            PlaylistViewModel.Instance.Songs = playlist.Songs;
                //        }
                //    });
                    
                    
               // });
            }
        }     
    }
}
