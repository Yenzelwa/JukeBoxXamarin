using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.ViewModels;
using JukeBox.Services;
using JukeBox.Models.Profile;
using JukeBox.Interfaces;

namespace JukeBox.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShuffleControl : ContentView
    {
        public ShuffleControl()
        {
            this.BindingContext = MusicStateViewModel.Instance;
            InitializeComponent();
            var playlistViewModel = MainViewModel.GetInstance().PlaylistViewModel;
            if(playlistViewModel != null)
            {
                if (playlistViewModel.Shuffle)
                {
                    shuffleimg.Source = ImageSource.FromFile("shuffle_on.png");
                }
                else
                {
                    shuffleimg.Source = ImageSource.FromFile("shuffle_off.png");
                }
            }
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((CachedImage)sender).Opacity = 0.6;
            ((CachedImage)sender).FadeTo(1, 150);
           
            var dataService = new DataService();
            var shuffleRepeat = dataService.GetSongShuffleRepeat();
            var _vm = MusicStateViewModel.Instance;
            var playlistViewModel = MainViewModel.GetInstance().PlaylistViewModel;
            if (playlistViewModel != null)
            {
                if (shuffleRepeat != null)
                {

                    if (playlistViewModel.Shuffle)
                    {
                        var isUpdateSongShuffleRepeat = new SongShuffleRepeat
                        {
                            Shuffle = false,
                            Repeat = shuffleRepeat.Repeat

                        };
                        dataService.Update(isUpdateSongShuffleRepeat);
                        shuffleimg.Source = ImageSource.FromFile("shuffle_off.png");
                        playlistViewModel.Shuffle = false;
                        var songs = playlistViewModel.Songs;
                        await DependencyService.Get<IMusicManager>().SetQueue(
                    songs);
                       

                    }
                    else
                    {
                        var updateSongShuffleRepeat = new SongShuffleRepeat
                        {
                            Shuffle = true,
                            Repeat = shuffleRepeat.Repeat

                        };
                        dataService.Update(updateSongShuffleRepeat);
                        shuffleimg.Source = ImageSource.FromFile("shuffle_on.png");
                        _vm.ShuffleCommand.Execute(null);
                        playlistViewModel.Shuffle = true;
                    }
                }
                else
                {
                    var songShuffleRepeat = new SongShuffleRepeat
                    {
                        Shuffle = true,
                        Repeat = false

                    };
                    dataService.Insert(songShuffleRepeat);
                    shuffleimg.Source = ImageSource.FromFile("shuffle_on.png");
                    _vm.ShuffleCommand.Execute(null);
                    
                    playlistViewModel.Shuffle = true;
                }
            }
        }
    }
}