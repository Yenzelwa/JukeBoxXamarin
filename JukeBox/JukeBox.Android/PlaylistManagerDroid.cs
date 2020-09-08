using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using JukeBox;
using Android.Provider;
using Android.Database;
using System.Collections.ObjectModel;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using System.IO;
using System.Net.Http;
using System.Net;
using System.ComponentModel;
using JukeBox.Interfaces;
using JukeBox.Models;
using Java.IO;
using Javax.Crypto;
using Javax.Crypto.Spec;
using static Android.Resource;
using JukeBox.Droid.FileEncryption;
using JukeBox.Droid;
using Android.Database.Sqlite;
using JukeBox.Helpers;
using JukeBox.Services;

[assembly: Dependency(typeof(PlaylistManagerDroid))]
namespace JukeBox
{
    public class PlaylistManagerDroid : IPlaylistManager
    {
        long stopTime, startTime;
        private string sKey = "0123456789abcdef";//key，
        private string ivParameter = "1020304050607080";
        SQLiteDatabase db;
        private static string[] _mediaProjections =
        {
            MediaStore.Audio.Media.InterfaceConsts.Id,
            MediaStore.Audio.Media.InterfaceConsts.Artist,
            MediaStore.Audio.Media.InterfaceConsts.Album,
            MediaStore.Audio.Media.InterfaceConsts.Title,
            MediaStore.Audio.Media.InterfaceConsts.Duration,
            MediaStore.Audio.Media.InterfaceConsts.Data,
            MediaStore.Audio.Media.InterfaceConsts.IsMusic,
            MediaStore.Audio.Media.InterfaceConsts.AlbumId
        };

        private static string[] _genresProjections =
        {
            MediaStore.Audio.Genres.InterfaceConsts.Name,
            MediaStore.Audio.Genres.InterfaceConsts.Id
        };

        private static string[] _albumProjections =
        {
            MediaStore.Audio.Albums.InterfaceConsts.Id,
            MediaStore.Audio.Albums.InterfaceConsts.AlbumArt
        };

        private static string[] _playlistProjections =
        {
            MediaStore.Audio.Playlists.InterfaceConsts.Id,
            MediaStore.Audio.Playlists.InterfaceConsts.Name,
            MediaStore.Audio.Playlists.InterfaceConsts.DateModified
        };

        private static string[] _playlistSongsProjections =
                {
                    MediaStore.Audio.Playlists.Members.AudioId,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.Artist,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.Title,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.IsMusic,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.Album,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.Duration,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.Title,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.Data,
                    MediaStore.Audio.Playlists.Members.InterfaceConsts.AlbumId
                };

       


        public bool EncryptFile(string filename, string path)
        {

            try
            {
              //  byte[] fileData = FileUtils.readFile("/storage/emulated/0/jukebox/Songs/" + filename);
              //  byte[] encodedBytes = EncryptDecryptUtils.encode(fileData);
              //  FileUtils.saveFile(encodedBytes, "/storage/emulated/0/jukebox/Songs/" + filename);
                return true;
            }
            catch (Exception e)
            {
                // updateUI("File Encryption failed.\nException: " + e.getMessage());
            }
            return false;
        }

        public byte[] DencryptFile(string filename, string path)
        {

            try
            {
              //  byte[] fileData = FileUtils.readFile("/storage/emulated/0/jukebox/Songs/" + filename);
               // byte[] decryptedBytes = EncryptDecryptUtils.decode(fileData);
              //  FileUtils.saveDecFile(decryptedBytes, "/storage/emulated/0/jukebox/Songs/" + filename, filename);
                return new byte[555];
            }
            catch (Exception e)
            {
                // updateUI("File Encryption failed.\nException: " + e.getMessage());
            }
            return null;
        }




        private void createFile(string filename, Java.IO.File extStore)
        {
            Java.IO.File file = new Java.IO.File(extStore + "/" + filename + ".aes");

            if (filename.IndexOf(".") != -1)
            {
                try
                {
                    file.CreateNewFile();
                }
                catch (Java.IO.IOException e)
                {
                    // TODO Auto-generated catch block
                    Android.Util.Log.Error("lv", e.Message);
                }
                Android.Util.Log.Error("lv", "file created");
            }
            else
            {
                file.Mkdir();
                Android.Util.Log.Error("lv", "folder created");
            }

            file.Mkdirs();
        }
        public void decrypt()
        {
            try
            {

                string path = "/storage/emulated/0/jukebox/Songs";
                //   Log.d("Files", "Path: " + path);
                Java.IO.File directory = new Java.IO.File(path);
                Java.IO.File[] files = directory.ListFiles();
                //  Log.d("Files", "Size: " + files.length);
                for (int i = 0; i < files.Length; i++)
                {
                    //  Log.d("Files", "FileName:" + files[i].getName());
                    var fileName = files[i].Name;
                    int index = fileName.LastIndexOf(".");
                    if (index > 0)
                        fileName = fileName.Substring(0, index);

                    //Java.IO.File extStore = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMovies);
                    Android.Util.Log.Error("Decryption Started", directory + "");
                    FileInputStream fis = new FileInputStream(directory + "/" + fileName + ".aes");

                    createFile(files[i].Name, directory);
                    FileOutputStream fos = new FileOutputStream(directory + "/" + "decrypted" + fileName, false);
                    System.IO.FileStream fs = System.IO.File.OpenWrite(directory + "/" + "decrypted" + fileName);
                    // Create cipher

                    Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");
                    byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
                    SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
                    IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
                    cipher.Init(Javax.Crypto.CipherMode.DecryptMode, skeySpec, iv);

                    startTime = System.DateTime.Now.Millisecond;
                    CipherOutputStream cos = new CipherOutputStream(fs, cipher);
                    int b;
                    byte[] d = new byte[1024 * 1024];
                    while ((b = fis.Read(d)) != -1)
                    {
                        cos.Write(d, 0, b);
                    }

                    stopTime = System.DateTime.Now.Millisecond;

                    Android.Util.Log.Error("Decryption Ended", directory + "/" + "decrypted" + fileName);
                    Android.Util.Log.Error("Time Elapsed", ((stopTime - startTime) / 1000.0) + "");

                    cos.Flush();
                    cos.Close();
                    fis.Close();
                }
            }
            catch (Exception e)
            {
                Android.Util.Log.Error("lv", e.Message);
            }
        }
        public async Task<IList<Song>> GetAllSongs()
        {
            //   decrypt();
            return await Task.Run<IList<Song>>(() =>
            {
                IList<Song> songs = new ObservableCollection<Song>();
                var db = new DataService();
                var files = db.GetAllSongs();
                // var duration = 4.5;
                foreach (var song in files)
                {
                    songs.Add(new Song
                    {
                        Id = (int)song.LibraryId,
                        Title = song.AudioTitle,
                        Artist = song.ArtistName,
                        Album = song.Album,
                        Genre = song.Genre,
                        Duration = 1000,
                        Uri = song.AudioData,
                        Artwork = song.ArtWork,
                        ImageSource = ImageSource.FromStream(() => new MemoryStream(song.ArtWork))

            });

                }
                return songs;
            });

            
           
        }

        public void decrypt(string filename)
        {
            try
            {

                Java.IO.File extStore = new Java.IO.File("/storage/emulated/0/jukebox/Songs");
                Android.Util.Log.Error("Decryption Started", extStore + "");
                FileInputStream fis = new FileInputStream(extStore + "/" + filename + ".aes");

                createFile(filename, extStore);
                FileOutputStream fos = new FileOutputStream(extStore + "/" + "decrypted" + filename, false);
                System.IO.FileStream fs = System.IO.File.OpenWrite(extStore + "/" + "decrypted" + filename);
                // Create cipher

                Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");
                byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
                SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
                IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
                cipher.Init(Javax.Crypto.CipherMode.DecryptMode, skeySpec, iv);

                startTime = System.DateTime.Now.Millisecond;
                CipherOutputStream cos = new CipherOutputStream(fs, cipher);
                Java.IO.File file = new Java.IO.File("/storage/emulated/0/jukebox/Songs" + "/" + filename);
                if (file.Delete())
                {
                    Android.Util.Log.Error("File Deteted", extStore + filename);
                }
                else
                {
                    Android.Util.Log.Error("File Doesn't exists", extStore + filename);
                }

                int b;
                byte[] d = new byte[1024 * 1024];
                while ((b = fis.Read(d)) != -1)
                {
                    cos.Write(d, 0, b);
                }

                stopTime = System.DateTime.Now.Millisecond;

                Android.Util.Log.Error("Decryption Ended", extStore + "/" + "decrypted" + filename);
                Android.Util.Log.Error("Time Elapsed", ((stopTime - startTime) / 1000.0) + "");

                cos.Flush();
                cos.Close();
                fis.Close();
            }
            catch (Exception e)
            {
                Android.Util.Log.Error("lv", e.Message);
            }
        }

     
        public async Task<IList<Albumlist>> GetSongsByAlbum()
        {
            return await Task.Run<IList<Albumlist>>(() =>
            {
                IList<Albumlist> albumlist = new ObservableCollection<Albumlist>();
                var db = new DataService();
                // var album = db.GetAbumById(albumId);
                var songs = db.GetAllSongs();
                var albumgroup = songs.GroupBy(x => new { Id = x.albumId, Name = x.Album }).Select(album => new
                {
                    AlbumId = album.Key.Id,
                    AlbumName = album.Key.Name,
                    Songs = album.ToList()
                });
                // var duration = 4.5;
                foreach (var album in albumgroup)
                {
                    var alb = new Albumlist
                    {
                        Id = (int)album.AlbumId,
                        Name = album.AlbumName,
                        Artwork = album.Songs[0].ArtWork,
                        ImageSource = ImageSource.FromStream(() => new MemoryStream(album.Songs[0].ArtWork))
                    };
                  //  var songs = db.GetSongById(alb.Id);
                    if (album.Songs != null)
                    {
                        alb.AlbumsSongs = new List<Song>();
                        foreach (var song in album.Songs)
                        {
                           
                            alb.AlbumsSongs.Add(new Song
                            {
                                Id = (int)song.LibraryId,
                                Title = song.AudioTitle,
                                Artist = song.ArtistName,
                                Album = song.Album,
                                Genre = song.Genre,
                                Duration = 1000,
                                Uri = song.AudioData,
                                Artwork = song.ArtWork,
                                ImageSource = ImageSource.FromStream(() => new MemoryStream(song.ArtWork))
                            });
                        }
                    }
                    albumlist.Add(alb);
                }
                return albumlist;
            });
        }

        public Playlist CreatePlaylist(string name , Song song)
        {
            //ContentValues contentValues = new ContentValues();
            //contentValues.Put(MediaStore.Audio.Playlists.InterfaceConsts.Name, name);
            //contentValues.Put(MediaStore.Audio.Playlists.InterfaceConsts.DateAdded, Java.Lang.JavaSystem.CurrentTimeMillis());
            //contentValues.Put(MediaStore.Audio.Playlists.InterfaceConsts.DateModified, Java.Lang.JavaSystem.CurrentTimeMillis());

            //Android.Net.Uri uri = Android.App.Application.Context.ContentResolver.Insert(
            //    MediaStore.Audio.Playlists.ExternalContentUri, contentValues);
            //if (uri != null)
            //{
            //    ICursor playlistCursor = Android.App.Application.Context.ContentResolver.Query(uri, _playlistProjections, null, null, null);
            //    if (playlistCursor.MoveToFirst())
            //    {
            //        ulong id = ulong.Parse(playlistCursor.GetString(playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.Id)));
            //        return new Playlist { Id = id, Title = name, Songs = new List<Song>(), IsDynamic = true, DateModified = DateTime.Now };
            //    }
            //    playlistCursor?.Close();

            //}
            //return null;
            var db = new DataService();
            var playlist = new PlaylistName
            {
                Title = name,
                DateModified = DateTime.Now
            };
            db.Insert(playlist);
            var inserted = db.GetInsertedPlaylst(name);
            if(inserted != null)
            {
                var playlistModel = new PlaylistModel
                {
                    playlistNameId = inserted.PlaylistNameId,
                    SongId = song.Id

                };
                db.Insert(playlistModel);
            }
            return null;
        }

        public async Task<IList<Song>> GetPlaylistSongs(ulong playlistId)
        {
            //return await Task.Run<IList<Song>>(() =>
            //{
            //    IList<Song> songs = new ObservableCollection<Song>();
            //    ICursor playlistCursor, songCursor;
            //    var url = MediaStore.Audio.Playlists.Members.GetContentUri("external", (long)playlistId);

            //    playlistCursor = Android.App.Application.Context.ContentResolver.Query(
            //        MediaStore.Audio.Playlists.ExternalContentUri,
            //        _playlistProjections,
            //        $"{MediaStore.Audio.Playlists.InterfaceConsts.Id} = {playlistId}",
            //        null, null);

            //    if (playlistCursor.MoveToFirst())
            //    {

            //        songCursor = Android.App.Application.Context.ContentResolver.Query(
            //            MediaStore.Audio.Playlists.Members.GetContentUri("external", (long)playlistId),
            //            _playlistProjections, null,
            //            null,null);

            //        int idColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.AudioId);


            //        ulong  id;



            //        if (songCursor.MoveToFirst())
            //        {
            //            do
            //            {

            //                id = ulong.Parse(songCursor.GetString(idColumn));

            //                var db = new DataService();
            //                var song  = db.GetFileById((int)id);
            //                songs.Add(new Song
            //                {
            //                    Id = (int)song.LibraryId,
            //                    Title = song.AudioTitle,
            //                    Artist = song.ArtistName,
            //                    Album = song.Album,
            //                    Genre = song.Genre,
            //                    Duration = 1000,
            //                    Uri = song.AudioData,
            //                    Artwork = song.ArtWork ,
            //                    ImageSource = ImageSource.FromStream(() => new MemoryStream(song.ArtWork != null ? song.ArtWork : null))
            //                });
            //            } while (songCursor.MoveToNext());
            //            songCursor?.Close();
            //        }
            //    }
            //    playlistCursor?.Close();

            //    return songs;
            //});
            return await Task.Run<IList<Song>>(() =>
            {
                IList<Song> songs = new ObservableCollection<Song>();
                var db = new DataService();
                var playSongs = db.GetSongPlaylist((int)playlistId);

                // var duration = 4.5;
                foreach (var playlist in playSongs)
                {
                    var files = db.GetSongsByPlaylistId((int)playlistId);
                    foreach (var song in files)
                    {
                        songs.Add(new Song
                        {
                            Id = (int)song.LibraryId,
                            Title = song.AudioTitle,
                            Artist = song.ArtistName,
                            Album = song.Album,
                            Genre = song.Genre,
                            Duration = 1000,
                            Uri = song.AudioData,
                            Artwork = song.ArtWork,
                            ImageSource = ImageSource.FromStream(() => new MemoryStream(song.ArtWork))

                        });

                    }               
                }
                return songs;
            });
               
        }

        public async  Task<IList<JukeBoxPlaylist>> GetPlaylists()
        {

            //IList<Playlist> playlists = new ObservableCollection<Playlist>();

            //ICursor playlistCursor = Android.App.Application.Context.ContentResolver.Query(
            //MediaStore.Audio.Playlists.ExternalContentUri,
            //_playlistProjections, null ,null,
            //MediaStore.Audio.Playlists.InterfaceConsts.Name);

            //int idColumn = playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.Id);
            //int nameColumn = playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.Name);
            //int dateModifiedColumn = playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.DateModified);

            //ulong id;
            //string name;
            //long time;

            //if (playlistCursor.MoveToFirst())
            //{
            //    do
            //    {
            //        id = ulong.Parse(playlistCursor.GetString(idColumn));
            //        name = playlistCursor.GetString(nameColumn);
            //        time = long.Parse(playlistCursor.GetString(dateModifiedColumn));

            //        DateTime dateModified = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(time);
            //        playlists.Add(new Playlist { Id = id, Title = name, IsDynamic = true, DateModified = dateModified });
            //    } while (playlistCursor.MoveToNext());
            //}
            //playlistCursor?.Close();
            //return playlists;
            return await Task.Run<IList<JukeBoxPlaylist>>(() =>
            {
                IList<JukeBoxPlaylist> playlists = new ObservableCollection<JukeBoxPlaylist>();
                var db = new DataService();
                var playlistList = db.GetAllPlaylist();
                if (playlistList.Count() > 0)
                {
                    foreach (var play in playlistList)
                    {

                        var playlist = new JukeBoxPlaylist
                        {
                            Id = (ulong)play.PlaylistNameId,
                            Title = play.Title,
                            PlaylistSongs = new List<Song>()
                        };
                        playlist.PlaylistSongs = new List<Song>();
                        var playlistModels = db.GetSongPlaylist((int)play.PlaylistNameId);
                        if (playlistModels.Count() > 0)
                        {
                            foreach (var playmodel in playlistModels)
                            {
                                var file = db.GetFileById(playmodel.SongId);
                                if (file != null)
                                {
                                    var song = new Song
                                    {
                                        Id = (int)file.LibraryId,
                                        Title = file.AudioTitle,
                                        Artist = file.ArtistName,
                                        Album = file.Album,
                                        Genre = file.Genre,
                                        Duration = 1000,
                                        Uri = file.AudioData,
                                        Artwork = file.ArtWork,
                                        ImageSource = ImageSource.FromStream(() => new MemoryStream(file.ArtWork))
                                    };
                                    playlist.PlaylistSongs.Add(song);
                                }
                                
                            }                    
                        }
                      
                        playlists.Add(playlist);
                        if(playlist.PlaylistSongs.Count > 0)
                        {
                            playlist.Artwork = playlist.PlaylistSongs[0].Artwork;
                            playlist.ImageSource = ImageSource.FromStream(() => new MemoryStream(playlist.PlaylistSongs[0].Artwork));
                        }
                        else
                        {
                            playlist.ImageSource = ImageSource.FromFile("playlist.png");
                        }
                       
                    }
                }
                return playlists;
            });

        }


        public void AddToPlaylist(JukeBoxPlaylist playlist, Song song)
        {
          
                var db = new DataService();
                var playlistModel = new PlaylistModel
                {
                    playlistNameId = (int)playlist.Id,
                    SongId = song.Id

                };
                db.Insert(playlistModel);
              //  return null;
        

            //await Task.Run(() =>
            //{
            //    ContentValues cv = new ContentValues();
            //    cv.Put(MediaStore.Audio.Playlists.Members.PlayOrder, 0);
            //    cv.Put(MediaStore.Audio.Playlists.Members.AudioId, song.Id);
            //    Android.Net.Uri uri = MediaStore.Audio.Playlists.Members.GetContentUri("external", (long)playlist.Id);
            //    ContentResolver resolver = Android.App.Application.Context.ContentResolver;
            //    var rUri = resolver.Insert(uri, cv);
            //    resolver.NotifyChange(Android.Net.Uri.Parse("content://media"), null);
            //});
        }

      
    }
}