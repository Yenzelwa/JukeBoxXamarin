using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JukeBox.Helpers;
using JukeBox.Models;
using JukeBox.Services;

namespace JukeBox.ViewModels
{
   public class LibraryDetailViewModel:BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public long LibraryId { get; set; }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        private ObservableCollection<ApiLibraryDetail> _libraryDetail { get; set; }
        public ObservableCollection<ApiLibraryDetail> LibraryDetail
        {
            get { return _libraryDetail; }
            set
            {
                _libraryDetail = value;
                OnPropertyChanged(nameof(LibraryDetail));
            }
        }
        #endregion

        #region Constructors
        public LibraryDetailViewModel()
        {
            this.apiService = new ApiService();
            
        }
        #endregion

        #region Methods
        #endregion
        public ICommand LibraryDetailCommand
        {
            get
            {
                return new RelayCommand(libraryDetail);
            }
        }

        public async void libraryDetail()
        {
            this.IsRunning = true;
            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
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
            var main = MainViewModel.GetInstance();
            var client = main.User.UserId > 0 ? main.User.UserId : 0;
            var response = await BLL.Library.Library.GetLibraryDetail(this.LibraryId ,client);



            if (response != null)
            {
                this.IsRunning = false;
                var lis = new ObservableCollection<ApiLibraryDetail>();
                foreach (var item in response.ResponseObject)
                {
                    item.IsStream = true;
                    lis.Add(item);
                }
                this.LibraryDetail = lis;

            }
            else
            {

                this.IsRunning = false;
                if (Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        checkConnetion.Message,
                        Languages.Accept);
                }
                return;
            }
        }
        #region Commands


        #endregion
    }
}

