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

[assembly: Dependency(typeof(PlaylistManagerDroid))]
namespace JukeBox
{
    public class PlaylistManagerDroid : IPlaylistManager
    {
        long stopTime, startTime;
        private string sKey = "0123456789abcdef";//key，
        private string ivParameter = "1020304050607080";
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

        public Playlist CreatePlaylist(string name)
        {
            ContentValues contentValues = new ContentValues();
            contentValues.Put(MediaStore.Audio.Playlists.InterfaceConsts.Name, name);
            contentValues.Put(MediaStore.Audio.Playlists.InterfaceConsts.DateAdded, Java.Lang.JavaSystem.CurrentTimeMillis());
            contentValues.Put(MediaStore.Audio.Playlists.InterfaceConsts.DateModified, Java.Lang.JavaSystem.CurrentTimeMillis());

            Android.Net.Uri uri = Android.App.Application.Context.ContentResolver.Insert(
                MediaStore.Audio.Playlists.ExternalContentUri, contentValues);
            if (uri != null)
            {
                ICursor playlistCursor = Android.App.Application.Context.ContentResolver.Query(uri, _playlistProjections, null, null, null);
                if (playlistCursor.MoveToFirst())
                {
                    ulong id = ulong.Parse(playlistCursor.GetString(playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.Id)));
                    return new Playlist { Id = id, Title = name, Songs = new List<Song>(), IsDynamic = true, DateModified = DateTime.Now };
                }
                playlistCursor?.Close();

            }
            return null;
        }


        public bool EncryptFile(string filename, string path)
        {

            try
            {
                byte[] fileData = FileUtils.readFile("/storage/emulated/0/jukebox/Songs/" + filename);
                byte[] encodedBytes = EncryptDecryptUtils.encode(fileData);
                FileUtils.saveFile(encodedBytes, "/storage/emulated/0/jukebox/Songs/" + filename);
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
                byte[] fileData = FileUtils.readFile("/storage/emulated/0/jukebox/Songs/" + filename);
                byte[] decryptedBytes = EncryptDecryptUtils.decode(fileData);
               FileUtils.saveDecFile(decryptedBytes, "/storage/emulated/0/jukebox/Songs/" + filename , filename);
                return decryptedBytes;
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
                ICursor mediaCursor, genreCursor, albumCursor;
                     mediaCursor = Android.App.Application.Context.ContentResolver.Query(
                    MediaStore.Audio.Media.ExternalContentUri,
                    _mediaProjections, MediaStore.Audio.Media.InterfaceConsts.Data + " like ? ",
    new string[] { "%jukebox%" },
                    MediaStore.Audio.Media.InterfaceConsts.TitleKey);

                int artistColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Artist);
                int albumColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Album);
                int titleColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Title);
                int durationColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Duration);
                int uriColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Data);
                int idColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Id);
                int isMusicColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.IsMusic);
                int albumIdColumn = mediaCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.AlbumId);

                int isMusic;
                ulong duration, id;
                string artist, album, uri ,title,  genre, artwork, artworkId;

                if (mediaCursor.MoveToFirst())
                {
                    do
                    {
                        isMusic = int.Parse(mediaCursor.GetString(isMusicColumn));
                        if (isMusic != 0)
                        {
                            title = mediaCursor.GetString(titleColumn);
                            var decrypt = DencryptFile(title + ".mp3","");

                         //  var file = FileUtils.getTempFileDescriptor(title + ".mp3", decrypt);
                            artist = mediaCursor.GetString(artistColumn);
                            album = mediaCursor.GetString(albumColumn);
                            uri = "/storage/emulated/0/movies/" + title + ".mp3";
                            duration = ulong.Parse(mediaCursor.GetString(durationColumn));



                            id = ulong.Parse(mediaCursor.GetString(idColumn));
                            artworkId = mediaCursor.GetString(albumIdColumn);
                           // file.Delete();
                           // decrypt(title + ".mp3");

                            genreCursor = Android.App.Application.Context.ContentResolver.Query(
                                MediaStore.Audio.Genres.GetContentUriForAudioId("external", (int)id),
                                _genresProjections, null, null, null);
                            int genreColumn = genreCursor.GetColumnIndex(MediaStore.Audio.Genres.InterfaceConsts.Name);
                            if (genreCursor.MoveToFirst())
                            {
                                genre = genreCursor.GetString(genreColumn) ?? string.Empty;
                            }
                            else
                            {
                                genre = string.Empty;
                            }

                            albumCursor = Android.App.Application.Context.ContentResolver.Query(
                                MediaStore.Audio.Albums.ExternalContentUri,
                                _albumProjections,
                                $"{MediaStore.Audio.Albums.InterfaceConsts.Id}=?",
                                new string[] { artworkId },
                                null);
                            int artworkColumn = albumCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.AlbumArt);
                            if (albumCursor.MoveToFirst())
                            {
                                artwork = albumCursor.GetString(artworkColumn) ?? string.Empty;
                            }
                            else
                            {
                                artwork = string.Empty;
                            }

                            songs.Add(new Song
                            {
                                Id = id,
                                Title = title,
                                Artist = artist,
                                Album = album,
                                Genre = genre,
                                Duration = duration / 1000,
                                Uri = uri,
                                Artwork = artwork
                            });
                            genreCursor?.Close();
                            albumCursor?.Close();
                     
                        }
                    } while (mediaCursor.MoveToNext());
                }
                mediaCursor?.Close();

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
        public IList<Playlist> GetPlaylists()
        {

            IList<Playlist> playlists = new ObservableCollection<Playlist>();

                ICursor playlistCursor = Android.App.Application.Context.ContentResolver.Query(
                MediaStore.Audio.Artists.ExternalContentUri ,
                _playlistProjections, MediaStore.Audio.Media.InterfaceConsts.Data + " like ? ",
    new string[] { "%jukebox/Songs%" },
                MediaStore.Audio.Playlists.InterfaceConsts.Name);

            int idColumn = playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.Id);
            int nameColumn = playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.Name);
            int dateModifiedColumn = playlistCursor.GetColumnIndex(MediaStore.Audio.Playlists.InterfaceConsts.DateModified);

            ulong id;
            string name;
            long time;

            if (playlistCursor.MoveToFirst())
            {
                do
                {
                    id = ulong.Parse(playlistCursor.GetString(idColumn));
                    name = playlistCursor.GetString(nameColumn);
                    time = long.Parse(playlistCursor.GetString(dateModifiedColumn));

                    DateTime dateModified = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(time);

                    playlists.Add(new Playlist { Id = id, Title = name, IsDynamic = true , DateModified = dateModified});
                } while (playlistCursor.MoveToNext());
            }
            playlistCursor?.Close();
            return playlists;
        }

        public async Task<IList<Song>> GetPlaylistSongs(ulong playlistId)
        {
            return await Task.Run<IList<Song>>(() =>
            {
                IList<Song> songs = new ObservableCollection<Song>();
                ICursor playlistCursor, songCursor, genreCursor, albumCursor;


                playlistCursor = Android.App.Application.Context.ContentResolver.Query(
                    MediaStore.Audio.Playlists.ExternalContentUri,
                    _playlistProjections, MediaStore.Audio.Media.InterfaceConsts.Data + " like ? ",
    new string[] { "%jukebox/Songs%" }, null);

                if (playlistCursor.MoveToFirst())
                {
                    songCursor = Android.App.Application.Context.ContentResolver.Query(
                        MediaStore.Audio.Playlists.Members.GetContentUri("external", (long)playlistId),
                        _playlistSongsProjections,
                       MediaStore.Audio.Media.InterfaceConsts.Data + " like ? ",
    new string[] { "%jukebox/Songs%" },
                        MediaStore.Audio.Playlists.Members.PlayOrder);

                    int artistColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.InterfaceConsts.Artist);
                    int titleColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.InterfaceConsts.Title);
                    int albumColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.InterfaceConsts.Album);
                    int uriColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.InterfaceConsts.Data);
                    int durationColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.InterfaceConsts.Duration);
                    int idColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.AudioId);
                    int albumIdColumn = songCursor.GetColumnIndex(MediaStore.Audio.Playlists.Members.InterfaceConsts.AlbumId);


                    string artist, album, title, uri, artworkId, genre, artwork;
                    ulong duration, id;


                    if (songCursor.MoveToFirst())
                    {
                        do
                        {
                            title = songCursor.GetString(titleColumn);
                            var decrypt = DencryptFile(title + ".mp3", "");
                            var file = FileUtils.getTempFileDescriptor(title + ".mp3", decrypt);
                            artist = songCursor.GetString(artistColumn);
                            album = songCursor.GetString(albumColumn);
                            duration = ulong.Parse(songCursor.GetString(durationColumn));
                            uri = songCursor.GetString(uriColumn);
                            id = ulong.Parse(songCursor.GetString(idColumn));
                            artworkId = songCursor.GetString(albumIdColumn);

                            genreCursor = Android.App.Application.Context.ContentResolver.Query(
                                MediaStore.Audio.Genres.GetContentUriForAudioId("external", (int)id),
                                _genresProjections, MediaStore.Audio.Media.InterfaceConsts.Data + " like ? ",
    new string[] { "%jukebox/Songs%" }, null);
                            int genreColumn = genreCursor.GetColumnIndex(MediaStore.Audio.Genres.InterfaceConsts.Name);
                            if (genreCursor.MoveToFirst())
                            {
                                genre = genreCursor.GetString(genreColumn) ?? string.Empty;
                            }
                            else
                            {
                                genre = string.Empty;
                            }

                            albumCursor = Android.App.Application.Context.ContentResolver.Query(
                                MediaStore.Audio.Albums.ExternalContentUri,
                                _albumProjections,
                                $"{MediaStore.Audio.Albums.InterfaceConsts.Id}=?",
                                new string[] { artworkId },
                                null);
                            int artworkColumn = albumCursor.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.AlbumArt);
                            if (albumCursor.MoveToFirst())
                            {
                                artwork = albumCursor.GetString(artworkColumn) ?? string.Empty;
                            }
                            else
                            {
                                artwork = string.Empty;
                            }

                            songs.Add(new Song
                            {
                                Id = id,
                                Title = title,
                                Artist = artist,
                                Album = album,
                                Genre = genre,
                                Duration = duration / 1000,
                                Uri = uri,
                                Artwork = artwork
                            });

                            genreCursor?.Close();
                            albumCursor?.Close();
                        } while (songCursor.MoveToNext());
                        songCursor?.Close();
                    }
                }
                playlistCursor?.Close();

                return songs;
            });
            
        }

        public async Task AddToPlaylist(Playlist playlist, Song song)
        {
            await Task.Run(() =>
            {
                ContentValues cv = new ContentValues();
                cv.Put(MediaStore.Audio.Playlists.Members.PlayOrder, 0);
                cv.Put(MediaStore.Audio.Playlists.Members.AudioId, song.Id);
                Android.Net.Uri uri = MediaStore.Audio.Playlists.Members.GetContentUri("external", (long)playlist.Id);
                ContentResolver resolver = Android.App.Application.Context.ContentResolver;
                var rUri = resolver.Insert(uri, cv);
                resolver.NotifyChange(Android.Net.Uri.Parse("content://media"), null);
            });
            
        }




}
}