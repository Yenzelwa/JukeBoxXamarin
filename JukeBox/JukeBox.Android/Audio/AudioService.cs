﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using JukeBox.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using JukeBox.Helpers;
using Android.Support.V4.Media.Session;
using JukeBox.BroadcastRecievers;
using JukeBox.Droid;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Support.V4.Media;
using static Android.Support.V4.Media.App.NotificationCompat;
using static Android.AccessibilityServices.GestureDescription;
using JukeBox.Audio;
using Android.Telephony;
using Java.IO;
using Android.Provider;

namespace JukeBox.Droid.Audio
{
    [Service]
    [IntentFilter(new string[] { ActionStart, ActionPlay, ActionPause, ActionNext, ActionPrev, ActionToggle, ActionStop, ActionTryKill })]
    public class AudioService : Service,
        AudioManager.IOnAudioFocusChangeListener,
        MediaPlayer.IOnPreparedListener,
        MediaPlayer.IOnErrorListener,
        MediaPlayer.IOnCompletionListener,
        MediaPlayer.IOnSeekCompleteListener
    {
        public const string ActionStart = "com.jukebox.START";
        public const string ActionPlay = "com.jukebox.PLAY";
        public const string ActionPause = "com.jukebox.PAUSE";
        public const string ActionNext = "com.jukebox.NEXT";
        public const string ActionPrev = "com.jukebox.PREV";
        public const string ActionToggle = "com.jukebox.TOGGLE";
        public const string ActionStop = "com.jukebox.STOP";
        public const string ActionTryKill = "com.jukebox.TRY_KILL";
        public static string CHANNEL_ID = "JukeBoxChannel";

        private MediaPlayer _player;
        private IList<Song> _queue;
        private int _pos;
        private bool _isPreparing = false;
        private bool _isSeeking = false;
        private bool _startOnPrepared = true;
        private bool _playing = false;

        private Action<bool> _isPlaying;
        private Action<double> _getPosition;
        private Action<int> _getQueuePos;
        private Action<IList<Song>> _getQueue;

        private AudioServiceBinder _binder;
        private AudioManager _audioManager;
        private MediaSessionCompat _mediaSessionCompat;
        private MediaControllerCompat _mediaControllerCompat;
        private ComponentName _remoteComponentName;
        private Random _random;
        private SongComparer _comparer;
        private bool btnStop_Pressed;

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
        //Handle incoming phone calls
        private bool ongoingCall = false;
        private PhoneStateListener phoneStateListener;
        private TelephonyManager telephonyManager;
        public override void OnCreate()
        {
            base.OnCreate();
            _queue = new ObservableCollection<Song>();
            _pos = 0;
            _audioManager = (AudioManager)GetSystemService(AudioService);
            _random = new Random();
            _comparer = new SongComparer();
            InitializePlayer();
            RequestAudioFocus();
        }

        private void InitializePlayer()
        {
            _player?.Reset();
            _player?.Release();
            _player?.Dispose();
            _player = null;
            _player = new MediaPlayer();
            _player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);
            _player.SetAudioStreamType(Stream.Music);
            _player.SetOnPreparedListener(this);
            _player.SetOnErrorListener(this);
            _player.SetOnCompletionListener(this);
            _player.SetOnSeekCompleteListener(this);
        }

        public void Init(Action<bool> IsPlaying, Action<double> GetPosition, Action<int> GetQueuePos, Action<IList<Song>> GetQueue)
        {
            _isPlaying = IsPlaying;
            _getPosition = GetPosition;
            _getQueuePos = GetQueuePos;
            _getQueue = GetQueue;

            _isPlaying?.Invoke(_player != null && !_isPreparing ? _player.IsPlaying : false);
            _getQueuePos?.Invoke(_pos);
            _getQueue?.Invoke(_queue);
        }

        public void SetQueue(IList<Song> songs)
        {
            if (songs == null)
            {
                songs = new ObservableCollection<Song>();
            }

            if (!Enumerable.SequenceEqual(_queue, songs, _comparer))
            {
                _queue = songs;
                _getQueue?.Invoke(_queue);
            }
        }

        public void Prepare(int pos)
        {
            _startOnPrepared = false;
            Start(pos);
        }

        public void Start(int pos)
        {
            //   System.Diagnostics.Debug.WriteLine("Start()");
            if (pos >= 0 && pos < _queue.Count && !_isPreparing)
            {
                _isPreparing = true;
                _pos = pos;
                _getQueuePos(_pos);
                _player?.Reset();
                try
                {
                    var url = _queue[_pos].Uri;
                    File tempMp3 = File.CreateTempFile(_queue[pos].Artist, ".mp3", CacheDir);
                    tempMp3.DeleteOnExit();
                    var tt = tempMp3.Length();
                    FileOutputStream fos = new FileOutputStream(tempMp3);
                    fos.Write(url);
                    fos.Close();
                    FileInputStream fis = new FileInputStream(tempMp3);
                    MediaMetadataRetriever retriever = new MediaMetadataRetriever();
                    retriever.SetDataSource(tempMp3.Path);
                    long duration = Java.Lang.Long.ParseLong(retriever.ExtractMetadata(MetadataKey.Duration));
                    _queue[_pos].Duration = duration / 1000;
                    retriever.Release();
                    _player?.SetDataSource(fis.FD);
                    _player?.PrepareAsync();
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }
        private static byte[] readFileToByteArray(Java.IO.File file)
        {
            FileInputStream fis = null;
            // Creating a byte array using the length of the file
            // file.length returns long which is cast to int
            byte[] bArray = new byte[(int)file.Length()];
            try
            {
                fis = new FileInputStream(file);
                fis.Read(bArray);
                fis.Close();

            }
            catch (Java.IO.IOException ioExp)
            {
                ioExp.PrintStackTrace();
            }
            return bArray;
        }

        private File getCacheDir()
        {
            throw new NotImplementedException();
        }

        public void AddToEndOfQueue(IList<Song> songs)
        {
            if (_queue == null || _queue.Count == 0)
            {
                SetQueue(songs);
            }
            else
            {
                foreach (Song song in songs)
                {
                    _queue.Add(song);
                }
                _getQueue?.Invoke(_queue);
            }
        }

        public void PlayNext(Song song)
        {
            if (_queue == null || _queue.Count == 0)
            {
                var songs = new ObservableCollection<Song>();
                songs.Add(song);
                SetQueue(songs);
                Start(0);
            }
            else
            {
                _queue.Insert(_pos + 1, song);
               _getQueue?.Invoke(_queue);
            }
        }

        public void Play()
        {
            System.Diagnostics.Debug.WriteLine("Play()");
            if (_player != null && !_isPreparing && !_player.IsPlaying)
            {
                RequestAudioFocus();
                _player.Start();
                UpdatePlaybackState(PlaybackStateCompat.StatePlaying);
                StartNotification(false);
                UpdateMediaMetadataCompat();

                Task.Run(() =>
                {
                    _playing = true;
                    while (_playing && !_isPreparing)
                    {
                        double sPos = _player.CurrentPosition / 1000;
                        _getPosition(_player.CurrentPosition / 1000);
                        Thread.Sleep(250);
                    }
                });
            }
            _isPlaying?.Invoke((bool)_player?.IsPlaying);
            UpdatePlaybackState(PlaybackStateCompat.StatePlaying);
        }

        public void Pause()
        {
            System.Diagnostics.Debug.WriteLine("Pause()");
            if (_player != null && !_isPreparing && _player.IsPlaying)
            {
                _player.Pause();
                _playing = false;
                UpdatePlaybackState(PlaybackStateCompat.StatePaused);
            }
            _isPlaying?.Invoke((bool)_player?.IsPlaying);
        }

        public void Next()
        {
            System.Diagnostics.Debug.WriteLine("Next()");
            if (_queue.Count > 0)
            {
                _pos = (_pos + 1) % _queue.Count;
                Start(_pos);
            }
        }

        public void Prev()
        {
            System.Diagnostics.Debug.WriteLine("Prev()");
            if (_queue.Count > 0)
            {
                _pos--;
                if (_pos < 0)
                {
                    _pos = _queue.Count > 0 ? _queue.Count - 1 : 0;
                }
                Start(_pos);
            }
        }

        public void Shuffle()
        {
            System.Diagnostics.Debug.WriteLine("Shuffle()");
            _playing = false;
            int n = _queue.Count;
            int k;
            Song temp;

            while (n > 1)
            {
                n--;
                k = _random.Next(n + 1);
                temp = _queue[k];
                _queue[k] = _queue[n];
                _queue[n] = temp;
            }
            _getQueue(_queue);
        }

        public void Seek(double position)
        {
            if (!_isPreparing)
            {
                _player?.SeekTo((int)position * 1000);
            }
        }

        private void InitializeMediaSession()
        {
            try
            {
                if (_mediaSessionCompat == null)
                {
                    Intent intent = new Intent(ApplicationContext, typeof(MainActivity));
                    PendingIntent pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0, intent, 0);

                    _remoteComponentName = new ComponentName(PackageName, new AudioControlsBroadcastReceiver().ComponentName);
                    _mediaSessionCompat = new MediaSessionCompat(ApplicationContext, "JukeBox", _remoteComponentName, pendingIntent);
                    _mediaControllerCompat = new MediaControllerCompat(ApplicationContext, _mediaSessionCompat.SessionToken);
                }

                _mediaSessionCompat.Active = true;
                _mediaSessionCompat.SetCallback(new AudioServiceCallback((AudioServiceBinder)_binder));
                _mediaSessionCompat.SetFlags(MediaSessionCompat.FlagHandlesMediaButtons | MediaSessionCompat.FlagHandlesTransportControls);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private void UpdatePlaybackState(int state)
        {
            if (_mediaSessionCompat != null && _player != null)
            {
                try
                {
                    PlaybackStateCompat.Builder stateBuilder = new PlaybackStateCompat.Builder()
                        .SetActions(
                            PlaybackStateCompat.ActionPause |
                            PlaybackStateCompat.ActionPlay |
                            PlaybackStateCompat.ActionPlayPause |
                            PlaybackStateCompat.ActionSkipToNext |
                            PlaybackStateCompat.ActionSkipToPrevious |
                            PlaybackStateCompat.ActionStop
                        )
                        .SetState(state, (long)_player?.CurrentPosition, 1.0f, SystemClock.ElapsedRealtime());
                    _mediaSessionCompat.SetPlaybackState(stateBuilder.Build());

                    if (state == PlaybackStateCompat.StatePlaying || state == PlaybackStateCompat.StatePaused)
                    {
                        var autoclose = state == PlaybackStateCompat.StatePaused ? true : false;
                        StartNotification(autoclose);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }

        private async void StartNotification(bool autoclose)
        {
            if (_mediaSessionCompat != null)
            {
                Intent intent = new Intent(ApplicationContext, typeof(MainActivity));
                PendingIntent pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0, intent, PendingIntentFlags.UpdateCurrent);
                Song currentSong = _queue[_pos];

                Intent audioServiceIntent = new Intent(ApplicationContext, typeof(AudioService));
                audioServiceIntent.SetAction(ActionStop);
                PendingIntent pendingCancelIntent = PendingIntent.GetService(ApplicationContext, 1, audioServiceIntent, PendingIntentFlags.CancelCurrent);
                var builder = new NotificationCompat.Builder(this, CHANNEL_ID);
                if (autoclose)
                {
                    builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                   .SetContentTitle(currentSong.Title)
                   .SetContentText(currentSong.Artist)
                   .SetContentInfo(currentSong.Album)
                   .SetSmallIcon(Resource.Drawable.icon)
                   .SetContentIntent(pendingIntent)
                   .SetOngoing(false)
                   .SetVisibility(Android.Support.V4.App.NotificationCompat.VisibilityPublic)
                   .SetDefaults(Android.Support.V4.App.NotificationCompat.FlagAutoCancel)
                   .SetPriority(Android.Support.V4.App.NotificationCompat.PriorityMax);
                }
                else
                {
                    builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                   .SetContentTitle(currentSong.Title)
                   .SetContentText(currentSong.Artist)
                   .SetContentInfo(currentSong.Album)
                   .SetSmallIcon(Resource.Drawable.icon)
                   .SetContentIntent(pendingIntent)
                   .SetOngoing(true)
                   .SetVisibility(Android.Support.V4.App.NotificationCompat.VisibilityPublic)
                   .SetDefaults(Android.Support.V4.App.NotificationCompat.FlagAutoCancel)
                   .SetPriority(Android.Support.V4.App.NotificationCompat.PriorityMax);
                }

                Bitmap artwork;
                //if (currentSong.Artwork !=null)
                //{
                //    artwork = await BitmapFactory.DecodeByteArrayAsync(currentSong.Artwork, 0, currentSong.Artwork.Length);

                //}
                //else
                //{
                    artwork = await BitmapFactory.DecodeResourceAsync(ApplicationContext.Resources, Resource.Drawable.icon);
                
                builder.SetLargeIcon(artwork);

                builder.AddAction(GenerateActionCompat(Android.Resource.Drawable.IcMediaPrevious, "Prev", ActionPrev));
                AddPlayPauseActionCompat(builder);
                builder.AddAction(GenerateActionCompat(Android.Resource.Drawable.IcMediaNext, "Next", ActionNext));

                MediaStyle style = new MediaStyle();
                style.SetShowCancelButton(true);
                style.SetCancelButtonIntent(pendingCancelIntent);
                style.SetMediaSession(_mediaSessionCompat.SessionToken);
                style.SetShowActionsInCompactView(0, 1, 2);
                builder.SetStyle(style);
                if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                {
                    // Notification channels are new in API 26 (and not a part of the
                    // support library). There is no need to create a notification 
                    // channel on older versions of Android.
                    return;
                }

                var channel = new NotificationChannel(CHANNEL_ID, "JukeBox Notifications", NotificationImportance.Low)
                {
                    Description = "JukeBox appear in this channel"
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
                notificationManager.Notify(1, builder.Build());
                //  StartForeground(1, builder.Build())
            }
        }

        private Android.Support.V4.App.NotificationCompat.Action GenerateActionCompat(int icon, string title, string intentAction)
        {
            Intent intent = new Intent(ApplicationContext, typeof(AudioService));
            intent.SetAction(intentAction);

            PendingIntentFlags flags = PendingIntentFlags.UpdateCurrent;
            if (intentAction.Equals(ActionStop))
            {
                flags = PendingIntentFlags.CancelCurrent;
            }

            PendingIntent pendingIntent = PendingIntent.GetService(ApplicationContext, 1, intent, flags);

            return new Android.Support.V4.App.NotificationCompat.Action.Builder(icon, title, pendingIntent).Build();
        }

        private void AddPlayPauseActionCompat(Android.Support.V4.App.NotificationCompat.Builder builder)
        {
            if (_player.IsPlaying)
            {
                
                builder.AddAction(GenerateActionCompat(Android.Resource.Drawable.IcMediaPause, "Pause", ActionPause));

            }
            else
            {
                builder.AddAction(GenerateActionCompat(Android.Resource.Drawable.IcMediaPlay, "Play", ActionPlay));
            }
        }

        public void StopNotification()
        {
            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(ApplicationContext);
            notificationManager?.CancelAll();
        }

        private void UpdateMediaMetadataCompat(MediaMetadataRetriever metaRetriever = null)
        {
            if (_mediaSessionCompat != null)
            {
                MainActivity.Instance.RunOnUiThread(() =>
                {
                    MediaMetadataCompat.Builder builder = new MediaMetadataCompat.Builder();
                    var item = _mediaSessionCompat.SessionToken;
                    if (metaRetriever != null)
                    {
                        builder
                            .PutString(MediaMetadata.MetadataKeyAlbum, metaRetriever.ExtractMetadata(MetadataKey.Album))
                            .PutString(MediaMetadata.MetadataKeyArtist, metaRetriever.ExtractMetadata(MetadataKey.Artist))
                            .PutString(MediaMetadata.MetadataKeyDisplayTitle, metaRetriever.ExtractMetadata(MetadataKey.Title));
                    }
                    else
                    {
                        builder
                            .PutString(MediaMetadata.MetadataKeyAlbum, _queue[_pos].Album)
                            .PutString(MediaMetadata.MetadataKeyArtist, _queue[_pos].Artist)
                            .PutString(MediaMetadata.MetadataKeyDisplayTitle, _queue[_pos].Title);
                    }

                    if (_queue[_pos].Artwork!=null)
                    {
                        Bitmap artwork = BitmapFactory.DecodeByteArray(_queue[_pos].Artwork, 0, _queue[_pos].Artwork.Length); 
                        builder.PutBitmap(MediaMetadataCompat.MetadataKeyAlbumArt, artwork);
                        builder.PutBitmap(MediaMetadataCompat.MetadataKeyArt, artwork);
                    }

                    _mediaSessionCompat.SetMetadata(builder.Build());
                });
            }
        }

        private void UnregisterMediaSessionCompat()
        {
            try
            {
                if (_mediaSessionCompat != null)
                {
                    _mediaSessionCompat.Dispose();
                    _mediaSessionCompat = null;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case ActionPlay:
                    Play();
                    break;
                case ActionPause:
                    Pause();
                    break;
                case ActionNext:
                    Next();
                    break;
                case ActionPrev:
                    Prev();
                    break;
                case ActionToggle:
                    if (_player != null && !_isPreparing)
                    {
                        if (_player.IsPlaying)
                        {
                            Pause();
                        }
                        else
                        {
                            Play();
                        }
                    }
                    break;
                case ActionStop:
                    Stop();
                    break;
                case ActionTryKill:
                    if (_player == null || !_player.IsPlaying)
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                    break;
            }
            return StartCommandResult.Sticky;
        }

        public async Task Stop()
        {
            await Task.Run(() =>
            {
                if (_player == null)
                    return;
                if (_player.IsPlaying)
                {
                    Pause();
                }
                UpdatePlaybackState(PlaybackStateCompat.StateStopped);
                _player.Reset();
                StopNotification();
                StopForeground(true);
                //UnregisterMediaSessionCompat();
            });
        }

        public override IBinder OnBind(Intent intent)
        {
            _binder = new AudioServiceBinder(this);
            InitializeMediaSession();
            return _binder;
        }

        public  void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
        {
            switch (focusChange)
            {
                case AudioFocus.Gain:
                    if (_player != null)
                    {
                        if (!_player.IsPlaying && btnStop_Pressed == true)
                        {
                            Play();
                            UpdatePlaybackState(PlaybackStateCompat.StatePlaying);
                        }
                        _player.SetVolume(1.0f, 1.0f);
                    }
                    break;
                case AudioFocus.Loss:
                    if (_player != null)
                    {
                        if(_player.IsPlaying)
                        Pause();
                    }
                    break;
                case AudioFocus.LossTransient:
                    if (_player.IsPlaying)
                    {
                        btnStop_Pressed = true;
                        Pause();
                    }
                    else
                    {
                        btnStop_Pressed = false;
                        
                    }
                    break;
                case AudioFocus.LossTransientCanDuck:
                    if (_player != null && _player.IsPlaying)
                    {
                        _player.SetVolume(.1f, .1f);
                    }
                    break;
            }
        }

        public void OnCompletion(MediaPlayer mp)
        {
            System.Diagnostics.Debug.WriteLine("OnCompletion()");
            _playing = false;
            _isPlaying?.Invoke(false);
            UpdatePlaybackState(PlaybackStateCompat.StatePaused);
            Next();
        }

        public bool OnError(MediaPlayer mp, [GeneratedEnum] MediaError what, int extra)
        {
            System.Diagnostics.Debug.WriteLine("OnError()");
            _playing = false;
            System.Diagnostics.Debug.WriteLine(what);
            mp.Reset();
            _isPlaying.Invoke(false);
            UpdatePlaybackState(PlaybackStateCompat.StatePaused);
            return false;
        }

        public void OnPrepared(MediaPlayer mp)
        {
            System.Diagnostics.Debug.WriteLine("OnPrepared()");
            _isPreparing = false;
            if (_startOnPrepared)
            {
                Play();
            }
            _startOnPrepared = true;
        }

        public void OnSeekComplete(MediaPlayer mp)
        {
            _getPosition(mp.CurrentPosition / 1000);
        }

        public bool RequestAudioFocus()
        {
            _audioManager = (AudioManager)GetSystemService(AudioService);
            AudioFocusRequest audioFocusRequest;
            if (Build.VERSION.SdkInt > BuildVersionCodes.O)
            {
                audioFocusRequest = _audioManager.RequestAudioFocus(new AudioFocusRequestClass.Builder(AudioFocus.Gain)
                .SetAudioAttributes(new AudioAttributes.Builder().SetLegacyStreamType(Stream.Music).Build()).SetOnAudioFocusChangeListener(this)
                .Build());

            }
            else
            {
                audioFocusRequest = _audioManager.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
            }

            if (audioFocusRequest == AudioFocusRequest.Granted)
            {
                return true;
            }
            return false;
        }

    

    }
}
