using JukeBox.Controls;
using JukeBox.Helpers;
using JukeBox.Models.Profile;
using JukeBox.Services;
using JukeBox.ViewModels;
using JukeBox.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JukeBox
{
    public partial class App : Application
    {
        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }

        public static RootPage Master
        {
            get;
            internal set;
        }
        #endregion

        public App()
        {
            InitializeComponent();

            var a = MusicStateViewModel.Instance;
            var b = QueuePopup.Instance;
            var c = SliderControl.Instance;
            if (Settings.IsRemembered == "true")
            {
                var dataService = new DataService();
                var token = dataService.GetToken();
                // var token = new TokenResponse();

                if (token != null)
                {
                    var user = dataService.GetUser();
                    var shuffleRepeat = dataService.GetSongShuffleRepeat();
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = token;
                    mainViewModel.User = user;
                    if(shuffleRepeat != null)
                    {
                        mainViewModel.PlaylistViewModel.Repeat = shuffleRepeat.Repeat;
                        mainViewModel.PlaylistViewModel.Shuffle = true;
                    }
                   

                    //  mainViewModel.Lands = new LandsViewModel();
                    Application.Current.MainPage = new RootPage();
                }
                else
                {
                    this.MainPage = new NavigationPage(new LoginPage());
                }
            }
            else
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }
        }
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Application.Current.MainPage =
                                  new NavigationPage(new LoginPage()));
            }
        }

        public static async Task NavigateToProfile(FacebookResponse profile)
        {
            if (profile == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var apiService = new ApiService();
            var dataService = new DataService();

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var token = await apiService.LoginFacebook(
                apiSecurity,
                "/api",
                "/Users/LoginFacebook",
                profile);

            if (token == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var user = await apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                token.UserName);

            UserLocal userLocal = null;
            if (user != null)
            {
                userLocal = Converter.ToUserLocal(user , Convert.ToInt32(token.UserName));
                dataService.Delete(userLocal);
                dataService.Insert(userLocal);
                dataService.Delete(token);
                dataService.Insert(token);
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.User = userLocal;
            if (mainViewModel.User !=null)
            mainViewModel.ImageSource = mainViewModel.User.ImagePath;
            mainViewModel.Lands = new LandsViewModel();
            Application.Current.MainPage = new RootPage();
            Settings.IsRemembered = "true";

            mainViewModel.Lands = new LandsViewModel();
            Application.Current.MainPage = new RootPage();
        }
    }
}
