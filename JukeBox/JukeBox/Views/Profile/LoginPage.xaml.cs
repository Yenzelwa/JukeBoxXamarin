namespace JukeBox.Views
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using JukeBox.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public MainViewModel Main
        {
            get;
            set;
        }
        public LoginPage ()
		{
           //this.Main = new MainViewModel();
           //this.BindingContext = new MainViewModel();
            InitializeComponent ();
		}
        private void Login_Clicked(object sender, EventArgs e)
        {
            
                var login = new LoginViewModel();
              //  login.Email = txtEmail.Text;
                //login.Password = txtPassword.Text;
                login.Login();
            

                
            //   Navigation.PushPopupAsync(new NowPlayingPopup());
        }
    }
}