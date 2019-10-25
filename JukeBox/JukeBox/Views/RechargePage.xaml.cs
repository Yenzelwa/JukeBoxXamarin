using JukeBox.Helpers;
using JukeBox.Models.Profile;
using JukeBox.Services;
using JukeBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JukeBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RechargePage : ContentPage
    {
        public RechargePage()
        {
            InitializeComponent();
        }
        private async void BtnTopUp_OnClicked(object sender, EventArgs e)
        {
            var img = ((Button)sender);
            if (img.BindingContext is MainViewModel main)
            {

                var request = new VoucherRequest
                {
                    VoucherPin = txtpin.Text,
                    CustomerId = main.User.UserId

                };

                var apiService = new ApiService();

                var checkConnetion = await apiService.CheckConnection();
                if (!checkConnetion.IsSuccess)
                {
                    this.IsEnabled = true;
                    await DisplayAlert(
                        Languages.Error,
                        checkConnetion.Message,
                        Languages.Accept);
                    return;
                }
                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                var orderResponse = await apiService.TopUpVoucher(
               apiSecurity,
               "/api/account",
               "/customer/redeem",
               request);

                if (orderResponse == null)
                {
                    this.IsEnabled = true;
                    await DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);
                    return;
                }
                if (orderResponse.ResponseType == 1)
                {
                    var user = await apiService.GetUserByEmail(
                    apiSecurity,
                    "/api/account",
                    "/customer/getcustomer",
                    main.Token.TokenType,
                    main.Token.AccessToken,
                    main.User.UserId.ToString());
                    var userLocal = Converter.ToUserLocal(user,Convert.ToInt32(main.Token.UserName));
                    userLocal.Password = main.User.Password;
                    main.User = userLocal;
                    this.IsEnabled = true;
                    await DisplayAlert(Languages.Accept, orderResponse.ResponseMessage, Languages.Accept);
                    return;
                }
                else
                {
                    this.IsEnabled = true;
                    await DisplayAlert(Languages.Error, orderResponse.ResponseMessage, Languages.Accept);
                    return;
                }
            }

        }

    }
}