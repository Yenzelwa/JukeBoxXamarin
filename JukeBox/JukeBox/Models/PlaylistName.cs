using SQLite;
using System;

namespace JukeBox.Models
{
    public class PlaylistName
    {
        [PrimaryKey ,AutoIncrement]
        public int PlaylistNameId { get; set; }
        public string Title { get; set; }
        public DateTime DateModified { get; set; }
    }
}