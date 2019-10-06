namespace JukeBox.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;
    using Helpers;
    using System.Collections.ObjectModel;
    using JukeBox.Models;
    using JukeBox.Profile.ViewModels;

    public class MenuItemViewModel
    {
        private static INavigation _nav;
        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
           App.Master.IsPresented = false;
            var currentPageTitle = App.Master.Detail.Title;
            if (this.PageName != currentPageTitle)
            {
                if (this.PageName == "LoginPage")
                {
                    Settings.IsRemembered = "false";
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = null;
                    mainViewModel.User = null;
                    Application.Current.MainPage = new NavigationPage(
                        new LoginPage());
                }
                else if (this.PageName == "Home")
                {
                    var PlaylistItems = new ObservableCollection<PlaylistItem>();
                    PlaylistItems.Add(new PlaylistItem(
                    new Playlist { Title = "Home", IsDynamic = false }));
                    LandingTabbedPage page = new LandingTabbedPage(PlaylistItems[0]);
                    (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(page);

                }
                else if (this.PageName == "MyProfilePage")
                {
                    MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                    (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new MyProfilePage());

                }
                else if (this.PageName == "AboutPage")
                {
                    (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new AboutPage());
                }

                    
            
            }
     
           
        }
        
        #endregion
    }
}