
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Windows.Input;
using JukeBox.Helpers;
using JukeBox.Models;
using JukeBox.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JukeBox.Views;
using System.ComponentModel;
using System;

namespace JukeBox.ViewModels
{
   public class LibraryViewModel: BaseViewModel
    {

        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isEnabled;
        #endregion

        #region Properties
        private ObservableCollection<ApiLibrary> _library { get; set; }
        public ObservableCollection<ApiLibrary> Library
        {
            get { return _library; }
            set
            {
                _library = value;
                OnPropertyChanged(nameof(Library));
            }
        }


        public ObservableCollection<ApiLibraryType> _libraryType { get; set; }
        public ObservableCollection<ApiLibraryType> LibraryType
        {
            get { return _libraryType; }
            set
            {
                _libraryType = value;
                OnPropertyChanged(nameof(LibraryType));
            }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructors
        public LibraryViewModel(int type)
        {
            this.apiService = new ApiService();
            this.GetLibrary(type);
            //this.GetLibraryType();
        }
        #endregion




        private async void GetLibrary(int filter)
        {


            this.IsRunning = true;
            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                //if (Application.Current.MainPage != null)
                //{
                //    await Application.Current.MainPage.DisplayAlert(
                //        Languages.Error,
                //        checkConnetion.Message,
                //        Languages.Accept);
                //}

                this.IsRunning = false;

                //await Application.Current.MainPage.DisplayAlert(
                //    Languages.Error,
                //    checkConnetion.Message,
                //    Languages.Accept);
                return;
            }
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var main = MainViewModel.GetInstance();
            var client = 0;
            if (main.User != null)
            { 
            var id = Convert.ToInt32(main.Token.UserName);
            client = id > 0 ? id: 0;
           }
            var response = await BLL.Library.Library.GetLibrary(filter , client );



            if (response !=null)
            {
                //if(response.ResponseObject ==null && String.IsNullOrWhiteSpace(response.ResponseMessage))
                //{
                //    this.IsRunning = false;
                //    //if (Application.Current.MainPage != null)
                //    //{
                //    //    await Application.Current.MainPage.DisplayAlert(
                //    //        Languages.Error,
                //    //      Languages.ConnectionError2,
                //    //        Languages.Accept);
                //    //}
                //    return;
                //}
                this.IsRunning = false; 
                this.Library = response.ResponseObject;

            }
            else
            {
                this.IsRunning = false;
                //if (Application.Current.MainPage != null)
                //{
                //    await Application.Current.MainPage.DisplayAlert(
                //        Languages.Error,
                //        checkConnetion.Message,
                //        Languages.Accept);
                //}
                return;
            }

           


        }

    }
}
