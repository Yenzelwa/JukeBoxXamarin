
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
   public class LibraryTypeViewModel: BaseViewModel
    {

        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isEnabled;
        #endregion

        #region Properties


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
        public LibraryTypeViewModel(int type)
        {
            this.apiService = new ApiService();
            this.GetLibraryType();
        }
        #endregion


        private async void GetLibraryType()
        {



            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                if (Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        checkConnetion.Message,
                        Languages.Accept);
                }
                return;
            }
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await BLL.Library.Library.GetLibraryTypes();

            var model = new ApiLibraryType
            {
                TypeId = 0,
                TypeName = "ALL",
                Color = "Black"
               


            };
            var modelList = new ObservableCollection<ApiLibraryType>();
           
            if (response != null)
            {

                foreach (var item in response.ResponseObject)
                {
                    item.Color = item.TypeId == 0 ? "Black" : "Gray";
                    modelList.Add(item);
                }
                this.LibraryType = modelList;

            }
            else
            {

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.ResponseMessage,
                    Languages.Accept);
                return;
            }




        }


     
    }
}
