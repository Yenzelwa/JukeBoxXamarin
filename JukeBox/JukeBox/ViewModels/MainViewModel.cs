namespace JukeBox.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Helpers;
    using Models;
    using Xamarin.Forms;
    using JukeBox.Interfaces;
    using JukeBox.Models.Profile;
    using JukeBox.Profile.ViewModels;

    public class MainViewModel : BaseViewModel
    {
        private static MainViewModel _instance;
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                }
                return _instance;
            }
        }

        #region Attibrutes
        private UserLocal user;
        #endregion

        #region Properties
        public List<Land> LandsList
        {
            get;
            set;
        }

        public TokenResponse Token 
        { 
            get; 
            set; 
        }

        public ObservableCollection<MenuItemViewModel> Menus
        {
            get;
            set;
        }

        public UserLocal User
        {
            get { return this.user; }
            set { SetValue(ref this.user, value); }
        }
        #endregion

        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }

        public LandsViewModel Lands
        {
            get;
            set;
        }

        public LandViewModel Land
        {
            get;
            set;
        }

        public RegisterViewModel Register
        {
            get;
            set;
        }

        public MyProfileViewModel MyProfile
        {
            get;
            set;
        }

        public ChangePasswordViewModel ChangePassword
        {
            get;
            set;
        }
        public PlaylistViewModel Playlist
        {
            get;
            set;
        }
        //public LibraryViewModel LibraryModel
        //{
        //    get;
        //    set;
        //}
        public LibraryDetailViewModel LibraryDetailModel
        {
            get;
            set;
        }
        private PlaylistViewModel _playlistViewModel;
        public PlaylistViewModel PlaylistViewModel
        {
            get { return _playlistViewModel; }
            set
            {
                _playlistViewModel = value;
                OnPropertyChanged(nameof(PlaylistViewModel));
            }
        }
        
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            this.LibraryModel = new LibraryViewModel(1);
            this.LibraryPromoModel = new LibraryPromoViewModel(3);
            this.LibraryDetailModel = new LibraryDetailViewModel();
            this.PlaylistItems = new ObservableCollection<PlaylistItem>();
            PlaylistItems.Add(new PlaylistItem(
            new Playlist { Title = "Home", IsDynamic = false }));
            this.PlaylistViewModel = new PlaylistViewModel(PlaylistItems[0]);
            
           
         //   this.Library = this.LibraryModel.Library;
           // this.LibraryDetail = new LibraryDetailViewModel.LibraryDetail();
            this.LoadMenu();
           
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        private IList<PlaylistItem> _playlistItems;

        public IList<PlaylistItem> PlaylistItems
        {
            get { return _playlistItems; }
            set
            {
                _playlistItems = value;
                OnPropertyChanged(nameof(PlaylistItems));
            }
        }

        private LibraryViewModel _libraryModel;

        public LibraryViewModel LibraryModel
        {
            get { return _libraryModel; }
            set
            {
                _libraryModel = value;
                OnPropertyChanged(nameof(LibraryModel));
            }
        }
        private LibraryPromoViewModel _libraryPromoModel;

        public LibraryPromoViewModel LibraryPromoModel
        {
            get { return _libraryPromoModel; }
            set
            {
                _libraryPromoModel = value;
                OnPropertyChanged(nameof(LibraryPromoModel));
            }
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            PlaylistItems = new ObservableCollection<PlaylistItem>();
            PlaylistItems.Add(new PlaylistItem(
                new Playlist { Title = "Home", IsDynamic = false }));
            this.Menus = new ObservableCollection<MenuItemViewModel>();

            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_action_people",
                PageName = "MyProfilePage",
                Title = Languages.MyProfile
            });


            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_action_info",
                PageName = "AboutPage",
                Title = "About"
            });

            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_chevron_right",
                PageName = "LoginPage",
                Title = Languages.LogOut
            });
        }
   
    #endregion
}
}