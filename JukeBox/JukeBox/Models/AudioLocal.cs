using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Models
{
    public class AudioLocal
    {
            [PrimaryKey,AutoIncrement]
            public int AudioId { get; set; }
        public long LibraryId { get; set; }
        public string AudioTitle { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public long albumId { get; set; }
        public string ArtistName { get; set; }
            public byte[] AudioData { get; set; }
        public byte[] ArtWork { get; set; }

        public override int GetHashCode()
            {
                return AudioId;
            }
        
    }
}
