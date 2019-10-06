namespace JukeBox.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using JukeBox;
    using JukeBox.Models.Profile;
    using JukeBox.ViewModels;
    using JukeBox.Views;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LandItemViewModel : Land
    {
        #region Commands
        public ICommand SelectLandCommand
        {
            get
            {
                return new RelayCommand(SelectLand);
            }
        }

        private async void SelectLand()
        {
          //  MainViewModel.GetInstance().Land = new LandViewModel(this);
        //  await App.Navigator.PushAsync(new LandingTabbedPage());
        }
        #endregion
    }
}