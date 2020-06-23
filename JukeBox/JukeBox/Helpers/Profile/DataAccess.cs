namespace JukeBox.Helpers
{
    using Interfaces;
    using JukeBox.Models.Profile;
    using Models;
    using Plugin.Permissions;
    using SQLite.Net;
    using SQLiteNetExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Xamarin.Forms;

    public class DataAccess 
    {
        private SQLite.SQLiteConnection connection;
        private  string dbPath;
        private static object collisionLock = new object();
        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            this.dbPath = Path.Combine(config.DirectoryDB, "JukeBox.db3");
          //  this.connection = new SQLite.SQLiteConnection(this.dbPath);
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    database.CreateTable<UserLocal>();
                    database.CreateTable<TokenResponse>();
                    database.CreateTable<AudioLocal>();
                    database.CreateTable<SongShuffleRepeat>();
                    database.CreateTable<PlaylistName>();
                    database.CreateTable<PlaylistModel>();
                    

                }
            }
                
        }

        public void Insert<T>(T model)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    database.Insert(model);
                }
            }
        }

        public void Update<T>(T model)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    database.Update(model);
                }
            }
        }

        public void Delete<T>(T model)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    database.Delete(model);

                }
            }
        }

        public IEnumerable<TokenResponse> GetMembers()
         {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<TokenResponse>() select mem);
                    return members.ToList();
                }
            }
        }
        public IEnumerable<UserLocal> GetLocalMembers()
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<UserLocal>() select mem);
                    return members.ToList();
                }
            }
        }
        public SongShuffleRepeat GetSongShuffleRepeat()
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = database.Table<SongShuffleRepeat>().FirstOrDefault();
                    return members;
                }
            }
        }
        public PlaylistName GetInsertedPlaylst(string tittle)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = database.Table<PlaylistName>().FirstOrDefault(x=>x.Title == tittle);
                    return members;
                }
            }
        }

        public IEnumerable<AudioLocal> GetAllFiles()
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<AudioLocal>() select mem);
                    return members.ToList();
                }
            }
        }


                
        public IEnumerable<AudioLocal> GetSongByPlaylistId(int playlistId)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<AudioLocal>() where mem.AudioId == playlistId select mem);
                    return members.ToList();
                }
            }
        }
        public IEnumerable<PlaylistName> GetAllPlaylist()
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<PlaylistName>() select mem);
                    return members.ToList();
                }
            }
        }
        public IEnumerable<PlaylistModel> GetSongPlaylist(int playlistId)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<PlaylistModel>() where  mem.playlistNameId == playlistId  select mem);
                    return members.ToList();
                }
            }
        }
        public IEnumerable<PlaylistModel> GetPlaylistBySongId(int songId)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var members = (from mem in database.Table<PlaylistModel>() where mem.SongId == songId select mem);
                    return members.ToList();
                }
            }
        }
        public AudioLocal GetFileById(long Id)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    return database.Table<AudioLocal>().FirstOrDefault(m => m.LibraryId == Id);
                }
            }
        }


        public IEnumerable<AudioLocal> GetSongByAbumId(long Id)
        {
            lock (collisionLock)
            {

                using (var database = new SQLite.SQLiteConnection(this.dbPath))
                {
                    var song = database.Table<AudioLocal>().Where(x => x.albumId == Id).ToList();
                    return song;
                }
            }
            
        }

    }
}