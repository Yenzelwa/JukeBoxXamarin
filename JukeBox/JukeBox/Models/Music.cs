using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Models
{
    public class Music
    {
       public string CollectionArtistName { get; set; }
       public string CollectionTitle { get; set; }
        public string CollectionImage { get; set; }
        public decimal CollectionPrice { get; set; }


        public class MusicPair : Tuple<Music, Music>
        {
           public MusicPair(Music item1, Music item2)
               : base(item1, item2 ?? null) { }

        //    private static Person CreateEmptyModel()
        //    {
        //        return new Person { IsVisible = true };
        //    }
       }

    }
}
