using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.ViewModels;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedFeatured : TabbedPage
    {
        public TabbedFeatured()
        {
            InitializeComponent();
            var mainViewModel = MainViewModel.GetInstance();
            //var libraryTypes = mainViewModel.LibraryModel.LibraryType;
            //if (libraryTypes != null)
            //{
            //    foreach (var item in libraryTypes)
            //    {
            //        this.Children.Add(
            //        new FeaturedPage()
            //        {
            //            Title = item.TypeName,
            //            BackgroundColor = Color.White
            //        });
            //    }
            //}
             
        }
    }
}