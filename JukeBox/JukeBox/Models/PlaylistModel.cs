using SQLite;

namespace JukeBox.Models
{
    public class PlaylistModel
    {
        [PrimaryKey,AutoIncrement]
        public int PlaylistId { get; set; }
        public int playlistNameId { get; set; }
        public int SongId { get; set; }
    }
}