using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JukeBox.Models
{
  public  class JukeBoxPlaylist
    {
        public ulong Id { get; set; }
        public bool deletePlaylist {  get; set;}

        public string Title { get; set; }

        public IList<Song> PlaylistSongs { get; set; }

        public bool IsDynamic { get; set; }

        public DateTime DateModified { get; set; }

        public bool HasArtwork { get { return Artwork != null && !String.IsNullOrEmpty(Artwork.ToString()); } }

        public object Artwork { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
