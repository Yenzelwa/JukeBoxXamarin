using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JukeBox.Models;

namespace JukeBox.Interfaces
{
    public interface IPlaylistManager
    {
        Task AddToPlaylist(Playlist playlist, Song song);

        Playlist CreatePlaylist(string name);

        IList<Playlist> GetPlaylists();

        Task<IList<Song>> GetPlaylistSongs(ulong playlistId); 

        Task<IList<Song>> GetAllSongs();
    }
}
