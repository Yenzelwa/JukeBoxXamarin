using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Provider;
using Java.IO;
using Javax.Crypto;
using Javax.Crypto.Spec;
using JukeBox.BLL.Library;
using JukeBox.Controls;
using JukeBox.Helpers;
using JukeBox.Interfaces;
using JukeBox.Models;
using JukeBox.Services;
using JukeBox.ViewModels;
using JukeBox.Views.MyMusic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Graphics.Bitmap;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicDetailPage : ContentPage
    {

        bool isDownloading = true;
        private string trailorUrl;
        private PlaylistViewModel _vm;
        private MediaPlayer _player;
        private long libraryId;
        private long artistId;
        private bool DownloadAlbum;
        private string filePath;
        private string albumCover;
        private List<ApiLibraryDetail> apiLibraryDetails;
        private DataService dataService;
        long stopTime, startTime;
        private string sKey = "0123456789abcdef";//key，
        private string ivParameter = "1020304050607080";
        const int _downloadImageTimeoutInSeconds = 15;
        readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds) };
        public MusicDetailPage(ApiLibrary library)
        {
            InitializeComponent();
            GetUpComingMovie(library);
            _player?.Reset();
            _player?.Release();
            _player?.Dispose();
            _player = null;
            _player = new MediaPlayer();

        }
        private async void GetUpComingMovie(ApiLibrary library)
        {
            // GridMoviesDetail.IsVisible = false;

            this.dataService = new DataService();

            try
            {
                //SLLoader.IsVisible = true;
                var main = MainViewModel.GetInstance();
                var clientId = main.User.UserId > 0 ? main.User.UserId : 0;
                var response = await Library.GetLibraryDetail(library.Id, clientId);
                if (response != null )
                {
                    if (response.ResponseObject != null)
                    {
                        libraryId = library.Id;
                        var songs = response.ResponseObject;
                        LblArtist.Text = library.Artist;
                        LblGenre.Text = library.Type;
                        var price = Math.Round(library.Price ?? 0, 2);
                        LblPrice.Text = "R" + price;
                        LblAlbum.Text = library.Name;
                        LblDescription.Text = library.Description;
                        ImgDetail.Source = ImageSource.FromUri(new Uri(library.CoverFilePath));
                        BtnBuy.Text = library.Purchase;
                        filePath = library.FilePath;
                        albumCover = library.CoverFilePath;
                        DownloadAlbum = library.AlbumDownload;
                        if (library != null)
                        {
                            var mainViewModel = MainViewModel.GetInstance();
                            mainViewModel.LibraryDetailModel.LibraryId = library.Id;
                            mainViewModel.LibraryDetailModel.libraryDetail();
                            GridMoviesDetail.IsVisible = true;
                            // BindingContext = mainViewModel;
                            apiLibraryDetails = new List<ApiLibraryDetail>();
                            foreach (var item in response.ResponseObject)
                            {
                                item.IsStream = true;
                                apiLibraryDetails.Add(item);
                            }
                            if (apiLibraryDetails != null)
                            {
                                lblSongs.IsVisible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //SLLoader.IsVisible = false;

                throw;
            }

            finally
            {
                // SLLoader.IsVisible = false;

            }


        }

        private void BtnBookOrder_OnClicked(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new OrderPage(LblMovieName.Text, LblPrice.Text.Substring(1)));
        }

        private async void BtnTrailor_OnClicked(object sender, EventArgs e)
        {
            var items = apiLibraryDetails;
            var mainViewModel = MainViewModel.GetInstance();
            var apiService = new ApiService();

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            if (items != null)
            {
                var request = new PurchaseOrderRequest
                {
                    LibraryId = libraryId,
                    LibraryDetailId = 0,
                    ClientId = Convert.ToInt32(mainViewModel.Token.UserName),
                    UserId = 1
                };

                var checkConnetion = await apiService.CheckConnection();
                if (!checkConnetion.IsSuccess)
                {
                    this.IsEnabled = true;
                    await DisplayAlert(
                        Languages.Error,
                        checkConnetion.Message,
                        Languages.Accept);
                    return;
                }
                if (!DownloadAlbum)
                {

                    var orderResponse = await apiService.PurchaseOrder(
                   apiSecurity,
                   "/api/library",
                   "/purchase",
                   "khji",
                   "klkl",
                   request);

                    if (orderResponse != null)
                    {
                        if (orderResponse.ResponseType == 1)
                        {
                            var response = await BLL.Library.Library.GetLibrary(1, Convert.ToInt32(mainViewModel.Token.UserName));
                            if (response != null)
                                mainViewModel.LibraryModel.Library = response.ResponseObject;
                            SongListView.IsEnabled = false;
                            Download.IsVisible = true;
                            BtnBuy.IsEnabled = false;                         
                            foreach (var item in items)
                                {
                                var dataService = new DataService();
                                // ImgDetail.te = "download";
                                await Task.Run(() =>
                                    {
                                        var audiobyte = getArrayFromUrl(item.FilePath);
                                        var artWork = getArrayFromUrl(albumCover);
                                        
                                            var song = new AudioLocal
                                            {
                                                LibraryId = item.Id,
                                                albumId = libraryId,
                                                AudioTitle = item.Name,
                                                ArtistName = LblArtist.Text,
                                                Album = LblAlbum.Text,
                                                Genre = LblGenre.Text,
                                                AudioData = audiobyte,
                                                ArtWork = artWork
                                            };
                                            dataService.Insert(song);
                                        
                                        
                                    });                               
                            }

                            SongListView.IsEnabled = true;
                            Download.IsVisible = false;
                            BtnBuy.IsEnabled = true;

                        }
                        else
                        {                           
                            await DisplayAlert(Languages.Error, orderResponse.ResponseMessage, Languages.Accept);
                            return;

                        }
                    }
                }
                else
                {
                    SongListView.IsEnabled = false;
                    Download.IsVisible = true;
                    BtnBuy.IsEnabled = false;
                  

                        foreach (var item in items)
                        {
                            if (dataService.GetFileById(item.Id) == null)
                            {
                            await Task.Run(() =>
                            {

                                var audiobyteArray = getArrayFromUrl(item.FilePath);
                                var artWork = getArrayFromUrl(albumCover);
                               
                                    var songs =  new AudioLocal
                                                  {
                                                      LibraryId = item.Id,
                                                      albumId = libraryId,
                                                      AudioTitle = item.Name,
                                                      ArtistName = LblArtist.Text,
                                                      Album = LblAlbum.Text,
                                                      Genre = LblGenre.Text,
                                                      AudioData = audiobyteArray,
                                                      ArtWork = artWork
                                                  };

                                        dataService.Insert(songs);
                                   
                            });
                        }
                        SongListView.IsEnabled = true;
                        Download.IsVisible = false;
                        BtnBuy.IsEnabled = true;

                    }
                }
            }
            else
            {
                await DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);
                return;
            }

            var user = await apiService.GetUserByEmail(
              apiSecurity,
              "/api/account",
              "/customer/getcustomer",
              mainViewModel.Token.TokenType,
              mainViewModel.Token.AccessToken,
              mainViewModel.Token.UserName);
            mainViewModel.Login.registerDataService(user, mainViewModel.Token);
            mainViewModel.PlaylistViewModel.Songs = await DependencyService.Get<IPlaylistManager>().GetAllSongs();
            await DependencyService.Get<IMusicManager>().SetQueue(mainViewModel.PlaylistViewModel.Songs);
            mainViewModel.PlaylistViewModel.Album = await DependencyService.Get<IPlaylistManager>().GetSongsByAlbum();
        }
        private async void BtnSingleDownload_OnClicked(object sender, EventArgs e)
        {
            var img = ((Button)sender);
            var mainViewModel = MainViewModel.GetInstance();
            var apiService = new ApiService();
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            if (img.BindingContext is ApiLibraryDetail song)
            {

                var request = new PurchaseOrderRequest
                {
                    LibraryId = 0,
                    LibraryDetailId = song.Id,
                    ClientId = Convert.ToInt32(mainViewModel.Token.UserName),
                    UserId = 1
                };
                var checkConnetion = await apiService.CheckConnection();
                if (!checkConnetion.IsSuccess)
                {
                    this.IsEnabled = true;
                    await DisplayAlert(
                        Languages.Error,
                        checkConnetion.Message,
                        Languages.Accept);
                    return;
                }
                var orderResponse = new ApiResponse();
                if (!song.SongDownload)
                {

                    orderResponse = await apiService.PurchaseOrder(
                  apiSecurity,
                  "/api/library",
                  "/purchase",
                  "khji",
                  "klkl",
                  request);

                    if (orderResponse != null)
                    {
                        if (orderResponse.ResponseType == 1)
                        {
                         //   DowloadFile(song.FilePath, "Album", LblMovieName.Text);

                            var response = await BLL.Library.Library.GetLibrary(1, Convert.ToInt32(mainViewModel.Token.UserName));
                            if (response != null)
                                mainViewModel.LibraryModel.Library = response.ResponseObject;
                            if (dataService.GetFileById(song.Id) == null)
                            {
                                var viewCell = img.Parent as Grid;
                                var viewControls = viewCell.Children;
                                viewControls[2].IsVisible = true;
                                viewControls[5].IsVisible = true;
                                viewControls[3].IsVisible = false;
                                img.IsVisible = false;

                                await Task.Run(async () =>
                                {
                                    var audiobyte = getArrayFromUrl(song.FilePath);
                                    var artWork = getArrayFromUrl(albumCover);
                                        var songs = new List<AudioLocal>
                                             {
                                                  new AudioLocal
                                                  {
                                                    LibraryId = song.Id,
                                                    albumId  = libraryId,
                                                    ArtistName = LblArtist.Text,
                                                    AudioTitle = song.Name,
                                                    Album = LblAlbum.Text,
                                                    Genre = LblGenre.Text,
                                                    AudioData = audiobyte,
                                                    ArtWork = artWork
                                                  }
                                             };
                                        dataService.Insert(songs);

                                });

                                viewControls[2].IsVisible = false;
                                viewControls[5].IsVisible = false;
                                viewControls[3].IsVisible = true;
                                img.IsVisible = true;
                            }
                        }
                        else
                        {
                            await DisplayAlert(Languages.Error, orderResponse.ResponseMessage, Languages.Accept);
                            return;

                        }
                    }
                }
                else
                {

                  if(dataService.GetFileById(song.Id) == null)
                    {
                        var viewCell = img.Parent as Grid;
                        var viewControls = viewCell.Children;
                        viewControls[2].IsVisible = true;
                        viewControls[5].IsVisible = true;
                        viewControls[3].IsVisible = false;
                        img.IsVisible = false;
                        await Task.Run(async () =>
                        {
                            var audiobyteArray = getArrayFromUrl(song.FilePath);
                            var artWork = getArrayFromUrl(albumCover);
                           
                                var songs =  new AudioLocal
                                                  {
                                                    LibraryId = song.Id,
                                                    ArtistName = LblArtist.Text,
                                                    albumId  = libraryId,
                                                    AudioTitle = song.Name,
                                                    Album = LblAlbum.Text,
                                                    Genre = LblGenre.Text,
                                                    AudioData = audiobyteArray,
                                                    ArtWork = artWork
                                                 
                                             };
                                 dataService.Insert(songs);                         
                        });
                        viewControls[2].IsVisible = false;
                        viewControls[5].IsVisible = false;
                        viewControls[3].IsVisible = true;
                        img.IsVisible = true;
                    }
                       
                }
            }
            else
            {
                await DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);
                return;
            }

            var user = await apiService.GetUserByEmail(
              apiSecurity,
              "/api/account",
              "/customer/getcustomer",
              mainViewModel.Token.TokenType,
              mainViewModel.Token.AccessToken,
              mainViewModel.Token.UserName);
            mainViewModel.Login.registerDataService(user, mainViewModel.Token);
            mainViewModel.PlaylistViewModel.Songs = await DependencyService.Get<IPlaylistManager>().GetAllSongs();
            await DependencyService.Get<IMusicManager>().SetQueue(mainViewModel.PlaylistViewModel.Songs);
            mainViewModel.PlaylistViewModel.Album = await DependencyService.Get<IPlaylistManager>().GetSongsByAlbum();
        }   
        public  byte[] getArrayFromUrl(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            byte[] b = null;

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();

            if (request.HaveResponse)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var receiveStream = response.GetResponseStream();
                    using (System.IO.BinaryReader br = new System.IO.BinaryReader(receiveStream))
                    {
                        b = br.ReadBytes(100000000);
                        var tt = b.Length;
                        br.Close();
                    }
                }
                return b;
            }

            return b;
        }
   
        private void Payment_Clicked(object sender, EventArgs e)
        {
            //  PopupNavigation.Instance.PushAsync(new PaymentPagePopUp());

        }
        private Xamarin.Forms.Image currentImg = null;
        private bool _isClicked = false;
        private async void TapPausePlay_OnTapped(object sender, EventArgs e)
        {

            var mainMusic = MusicStateViewModel.Instance;
            if (mainMusic.IsPlaying)
            {
                DependencyService.Get<IMusicManager>().Pause();
            }
            _isClicked = !_isClicked;
            var img = ((Xamarin.Forms.Image)sender);

            if (img.BindingContext is ApiLibraryDetail libraryDetail)
            {


                if (_player != null && _player.IsPlaying)
                {
                    _player.Stop();
                    _player.Reset();

                    var v = img.Source.ToString();
                    _isClicked = img.Source.ToString() == "File: pause_w.png" ? false : true;
                    if (!_isClicked)
                    {
                        currentImg.Source = ImageSource.FromFile("play_w.png");
                    }
                    else
                    {
                        currentImg.Source = ImageSource.FromFile("play_w.png");
                    }
                }
                currentImg = img;
                if (_isClicked)
                {
                    _player = new MediaPlayer();
                    currentImg.IsVisible = false;
                    var viewCell = img.Parent as Grid;
                    var viewControls = viewCell.Children;
                    viewControls[1].IsVisible = true;
                    await Task.Run(async () => {

                        _player.SetAudioStreamType(streamtype: Android.Media.Stream.Music);
                        _player.SetDataSource(libraryDetail.FilePath);
                        _player?.Prepare();
                        _player?.Start();
                       
                        await Task.Delay(780000);
                    });
                    currentImg.Source = ImageSource.FromFile("pause_w.png");
                    viewControls[1].IsVisible = false;
                    currentImg.IsVisible = true;
                }


                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    if (_player != null && _player.CurrentPosition > 30000)
                    {
                        _player.Stop();
                        _player.Reset();
                        _player = null;
                        currentImg.Source = ImageSource.FromFile("play_w.png");
                    }

                    return true; // return true to repeat counting, false to stop timer
                });

            }


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



        protected override void OnDisappearing()
        {
            if (_player != null)
            {

                if (_player.IsPlaying)
                {
                    _player.Stop();
                    currentImg.Source = ImageSource.FromFile("play_w.png");
                    _isClicked = false;
                }
                _player.Release();
                _player = null;
            }

        }

        public void onSeekComplete(MediaPlayer mp)
        {
        }

    }
}