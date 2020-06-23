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
       void  AddToPlaylist(JukeBoxPlaylist playlist, Song song);

        Playlist CreatePlaylist(string name, Song song);

        Task<IList<JukeBoxPlaylist>> GetPlaylists();

        Task<IList<Song>> GetPlaylistSongs(ulong playlistId); 

        Task<IList<Song>> GetAllSongs();
        Task<IList<Albumlist>> GetSongsByAlbum();
        bool EncryptFile(string filename, string path);
    }
}
