namespace JukeBox.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using JukeBox.Models;
    using JukeBox.Models.Profile;

    public class DataService
    {
    
        public T Insert<T>(T model)
        {
            var da = new DataAccess();
           da.Insert(model);
             return model;
            
        }


        public UserLocal GetUser() 
        {
            var da = new DataAccess();
            
             return da.GetLocalMembers().FirstOrDefault();
           
        }
        public SongShuffleRepeat GetSongShuffleRepeat()
        {
            var da = new DataAccess();

            return da.GetSongShuffleRepeat();

        }
        public PlaylistName GetInsertedPlaylst(string tittle)
        {
            var da = new DataAccess();

            return da.GetInsertedPlaylst(tittle);

        }
        public TokenResponse GetToken()
        {
            var da = new DataAccess();
            return da.GetMembers().FirstOrDefault();
            
        }
        public IEnumerable<AudioLocal> GetAllSongs()
        {
            var da = new DataAccess();
            return da.GetAllFiles().ToList();
            
        }
        public AudioLocal GetFileById(long id)
        {
            var da = new DataAccess();
            return da.GetFileById(id);
            
        }
        public PlaylistModel GetPlaylistById(int songId , int playlistNameId)
        {
            var da = new DataAccess();
            return da.GetPlaylistId(songId,playlistNameId);
            
        }
       
        public IEnumerable<AudioLocal> GetSongById(long id)
        {
            var da = new DataAccess();
            return da.GetSongByAbumId(id);
            
        }


        public IEnumerable<AudioLocal> GetSongsByPlaylistId(int playlistId)
        {
            var da = new DataAccess();
            return da.GetSongByPlaylistId(playlistId);     
        }
        public IEnumerable<PlaylistName> GetAllPlaylist()
        {
            var da = new DataAccess();
            return da.GetAllPlaylist();
        }
        public IEnumerable<PlaylistModel> GetSongPlaylist(int playlistId)
        {
            var da = new DataAccess();
            return da.GetSongPlaylist(playlistId);
        }
        public IEnumerable<PlaylistModel> GetPlaylistBySongId(int songId)
        {
            var da = new DataAccess();
            return da.GetPlaylistBySongId(songId);
        }
        

        public void Update<T>(T model)
        {
            var da = new DataAccess();
            da.Update(model);
            
        }

        public void Delete<T>(T model)
        {
            var da = new DataAccess();
            da.Delete(model);
            
        }

        public void Save<T>(List<T> list) where T : class
        {
            var da = new DataAccess();
            foreach (var record in list)
                {
                    Insert(record);
                   // InsertOrUpdate(record);
                }
            }
        
    }
}