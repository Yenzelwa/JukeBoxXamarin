using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JukeBox.Controller
{
  public  class MusicAppNavigationPage: NavigationPage
    {
        public MusicAppNavigationPage(Page root) : base(root)
        {
            Init();
        }

        public MusicAppNavigationPage()
        {
            Init();
        }

        void Init()
        {

            BarBackgroundColor = Color.FromHex("#03A9F4");
            BarTextColor = Color.White;
        }
    }
}
