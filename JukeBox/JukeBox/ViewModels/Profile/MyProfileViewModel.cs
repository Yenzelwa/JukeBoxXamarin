﻿namespace JukeBox.Profile.ViewModels
{
    using System;
    using System.IO;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using JukeBox;
    using JukeBox.Helpers;
    using JukeBox.Models.Profile;
    using JukeBox.Services;
    using JukeBox.ViewModels;
    using JukeBox.Views;
    using Models;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Xamarin.Forms;
 

    public class MyProfileViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public UserLocal User
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MyProfileViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            var v = MainViewModel.GetInstance();
            this.User = MainViewModel.GetInstance().User;
            if (this.User != null) {
                if (User.ImageArray != null)
                {
                    this.ImageSource = ImageSource.FromStream(() => new MemoryStream(User.ImageArray));
                }
                else
                {
                    this.ImageSource = "no_image";
                }
            }
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand
        {
            get
            {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword()
        {
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
            await App.Navigator.PushAsync(new ChangePasswordPage());
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    Languages.SourceImageQuestion,
                    Languages.Cancel,
                    null,
                    Languages.FromGallery,
                    Languages.FromCamera);

                if (source == Languages.Cancel)
                {
                    this.file = null;
                    return;
                }

                if (source == Languages.FromCamera)
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                User.ImageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.ImageSource = ImageSource.FromStream(() => new MemoryStream(User.ImageArray));

            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (!RegexUtilities.IsValidEmail(this.User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation2,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.User.Telephone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneValidation,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Message,
                    Languages.Accept);
                return;
            }

      
          
            var userDomain = Converter.ToUserDomain(this.User);
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Post(
                apiSecurity,
                "/api/account",
                "/customer",
                userDomain);

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

            var userApi = await this.apiService.GetUserByEmail(
                apiSecurity,
                 "/api/account",
                "/customer/getcustomer",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                MainViewModel.GetInstance().Token.UserName);
            userApi.ImageArray = User.ImageArray;
            var userLocal = Converter.ToUserLocal(userApi, Convert.ToInt32(MainViewModel.GetInstance().Token.UserName));

            MainViewModel.GetInstance().User = userLocal;
            MainViewModel.GetInstance().Login.registerDataService(userApi, MainViewModel.GetInstance().Token);
            MainViewModel.GetInstance().ImageSource = ImageSource.FromStream(() => new MemoryStream(User.ImageArray));
            this.dataService.Update(userLocal);

            this.IsRunning = false;
            this.IsEnabled = true;
            await Application.Current.MainPage.DisplayAlert(
               Languages.ConfirmLabel,
               "Profile Updated Succefully",
               Languages.Accept);
            //  await App.Navigator.PopAsync();
        }
        #endregion
    }
}