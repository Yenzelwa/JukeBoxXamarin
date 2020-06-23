using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;
using JukeBox.ViewModels;
using Xamarin.Forms.Xaml;

namespace JukeBox.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicBarControl : ContentView
    {
        public MusicBarControl()
        {
            InitializeComponent();
            this.BindingContext = MusicStateViewModel.Instance;
         
            //  Carousel.Position = MusicStateViewModel.Instance.QueuePos;
            var main = MainViewModel.GetInstance();
            //if(main.PlaylistViewModel.Songs.Count>0)
            //{
            //    main.PlaylistViewModel.MusicState.HasSongs = true;
            //    gridMusicBar.IsVisible = true;
            //}
        }

        private void OpenNowPlayingPopup(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new NowPlayingPopup());
        }
    }
}
