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
	public partial class CustomNavigationBar : ContentView
	{
		public CustomNavigationBar ()
		{
			InitializeComponent ();
		}
        public Label FirstNameLabel
        {

            get
            {
                return labelText;
            }
            
        }
        
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
        "Title",
        typeof(string),
        typeof(CustomNavigationBar),
        "this is Title",
        propertyChanged: OnTitlePropertyChanged
        );
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as CustomNavigationBar;
            var title = newValue.ToString();
            thisView.lblTitle.Text = title;
        }
        private void Tapcart_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HomeTabbedPage());
        }
    }
}