
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
using JukeBox.ViewModels;
using System;

namespace JukeBox.Models
{
  public  class ApiLibraryType 
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Color { get; set; }
        public ICommand LibraryCommand
        {
            get
            {
                return new RelayCommand(GetLibrary);
            }
        }

        private async void GetLibrary()
        {

            if (this.TypeId > -1 )
            {
                var mainViewModel = MainViewModel.GetInstance();
                var modelList = new ObservableCollection<ApiLibraryType>();
                foreach (var item in mainViewModel.LibraryTypeModel.LibraryType)
                {
                    item.Color = item.TypeId == this.TypeId ? "Black" : "Gray";
                    modelList.Add(item);
                }
                mainViewModel.LibraryTypeModel.LibraryType = modelList;
                mainViewModel.LibraryModel = new LibraryViewModel(this.TypeId);

            }
            
        }
    }
}
