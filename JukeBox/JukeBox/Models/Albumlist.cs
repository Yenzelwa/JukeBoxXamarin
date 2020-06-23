using GalaSoft.MvvmLight.Command;
using JukeBox.Views;
using JukeBox.Views.MyMusic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JukeBox.Models
{
   public class Albumlist
    {

        public ICommand AlbumSongs
        {
            get
            {
                return new RelayCommand(AlbumDetail);
            }
        }
        public long Id { get; set; }

        public string Name { get; set; }

        public IList<Song> AlbumsSongs { get; set; }


        public bool HasArtwork { get { return Artwork != null && !String.IsNullOrEmpty(Artwork.ToString()); } }

        public byte[] Artwork { get; set; }
        public ImageSource ImageSource { get; set; }
        private  void AlbumDetail()
        {
             Application.Current.MainPage.Navigation.PushAsync(new SonglistPage(this));
        }

       
    }
}
