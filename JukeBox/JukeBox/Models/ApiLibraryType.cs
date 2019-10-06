
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
        public ICommand LibraryCommand
        {
            get
            {
                return new RelayCommand(GetLibrary);
            }
        }

        private async void GetLibrary()
        {

            if (this.TypeId > 0 )
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.LibraryModel = new LibraryViewModel(this.TypeId);

            }
            
        }
    }
}
