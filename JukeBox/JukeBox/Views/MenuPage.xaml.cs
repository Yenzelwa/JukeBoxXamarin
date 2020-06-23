using System;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JukeBox.ViewModels;
using JukeBox.Controls;
using JukeBox.Models;
using System.IO;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
         //public ListView PlaylistList => ListViewMenuItems;
        public MainViewModel Main
        {
            get;
            set;
        }
        public MenuPage()
        {
             this.Main = MainViewModel.GetInstance();
            // this.BindingContext = this.Main;
            if (this.Main.User != null)
                if (Main.User.ImageArray != null)
                {
                    Main.ImageSource = ImageSource.FromStream(() => new MemoryStream(Main.User.ImageArray));
                }
                else
                {
                    Main.ImageSource = "no_image";
                }
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new CreatePlaylistPopup(null), true);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 0.6;
            ((Image)sender).FadeTo(1, 150);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            foreach (PlaylistItem item in MenuViewModel.Instance.PlaylistItems)
            {
                item.IsToggled = ((Switch)sender).IsToggled;
            }
        }
    }
}
