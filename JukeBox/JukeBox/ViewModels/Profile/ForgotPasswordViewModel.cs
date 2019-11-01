namespace JukeBox.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using JukeBox;
    using JukeBox.Helpers;
    using JukeBox.Models.Profile;
    using JukeBox.Services;
    using JukeBox.ViewModels;
    using JukeBox.Views;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public string Email
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        public ForgotPasswordViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();

            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ForgotPasswordCommand
        {
            get
            {
                return new RelayCommand(ForgotPassword);
            }
        }

        private async void ForgotPassword()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (!RegexUtilities.IsValidEmail(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation2,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var request = new ChangePasswordRequest
            {
                CurrentPassword = null,
                Email = this.Email,
                NewPassword = null,
            };

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.ForgotPassword(
                apiSecurity,
                "/api/account",
                "/forgotpassword",
                request.Email);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

          //  MainViewModel.GetInstance().User.Password = this.NewPassword;
         //   this.dataService.Update(MainViewModel.GetInstance().User);

            this.IsRunning = false;
            this.IsEnabled = true;
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage());
        }
        #endregion
    }
}

