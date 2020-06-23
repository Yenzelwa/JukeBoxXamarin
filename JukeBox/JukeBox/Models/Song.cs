
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JukeBox.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Genre { get; set; }

        public byte[] Artwork { get; set; }

        public double Duration { get; set; }

        public decimal? Price { get; set; }

        //public DateTime Date { get; set; }

        public byte[] Uri { get; set; }
        public ImageSource ImageSource { get; set; }

        public bool HasArtwork
        {
            get
            {
                return Artwork != null && !String.IsNullOrEmpty(Artwork.ToString());
            }
        }

        public Song() { }

        public Song(Song song)
        {
            Id = song.Id;
            Title = song.Title;
            Artist = song.Artist;
            Album = song.Album;
            Genre = song.Genre;
            Artwork = song.Artwork;
            Duration = song.Duration;
            Uri = song.Uri;
        }
    }
}
