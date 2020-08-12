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
using System.Linq;

namespace JukeBox.ViewModels
{
    public class LibraryPromoViewModel : BaseViewModel
    {

        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        public bool isEnabled;
        #endregion

        #region Properties
        private ObservableCollection<PromotionType> _promotionType { get; set; }
        public ObservableCollection<PromotionType> PromotionType
        {
            get { return _promotionType; }
            set
            {
                _promotionType = value;
                OnPropertyChanged(nameof(PromotionType));
            }
        }
        private ObservableCollection<PromotionCategory> _promotionCategory { get; set; }
        public ObservableCollection<PromotionCategory> PromotionCategory
        {
            get { return _promotionCategory; }
            set
            {
                _promotionCategory = value;
                OnPropertyChanged(nameof(PromotionCategory));
            }
        }
        private ObservableCollection<PromotionResult> _promotionResult { get; set; }
        public ObservableCollection<PromotionResult> PromotionResult
        {
            get { return _promotionResult; }
            set
            {
                _promotionResult = value;
                OnPropertyChanged(nameof(PromotionResult));
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
        public LibraryPromoViewModel()
        {
            this.apiService = new ApiService();
          this.GetPromotionType();
        }
        #endregion




        public async void GetPromotionResult(int promotionTypeId, int promoMapId)
        {


            this.IsRunning = true;
            var response = await BLL.Library.Library.GetPromotionResult(promotionTypeId, promoMapId);
            if (response != null)
            {
                this.PromotionResult = response.ResponseObject;
            }

            this.IsRunning = false;
        }
        public async void GetPromotionCategory(int promotionTypeId)
        {


            this.IsRunning = true;
            var response = await BLL.Library.Library.GetPromotionCategory(promotionTypeId);
            if (response != null)
            {
                this.PromotionCategory = response.ResponseObject;
            }

            this.IsRunning = false;
        }
        private async void GetPromotionType()
        {
            this.IsRunning = true;
           
            var response = await BLL.Library.Library.GetPromotionType();
            if(response !=null)
            {
                this.PromotionType = response.ResponseObject;
            }
            this.IsRunning = false;

        }
        private async void GetLibraryType()
        {



            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                //if(Application.Current.MainPage !=null)
                //{
                //    await Application.Current.MainPage.DisplayAlert(
                //        Languages.Error,
                //        checkConnetion.Message,
                //        Languages.Accept);
                //}

                return;
            }
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await BLL.Library.Library.GetLibraryTypes();



            if (response != null)
            {
             //   this.LibraryType = response.ResponseObject;

            }
            else
            {

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

