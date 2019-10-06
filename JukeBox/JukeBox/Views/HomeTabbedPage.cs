
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JukeBox.Views
{
  public  class HomeTabbedPage :TabbedPage
    {
        //public IList<Page> Children { get; }
        public HomeTabbedPage()
        {
            Children.Add(new FeaturedPage()
                    {
                Title = "FEATURED",
                        Icon = Xamarin.Forms.Device.OnPlatform<string>("tab_feed.png", null, null)
                    });
            //Children.Add(new BlogPage()
            //{
            //    Title = "BLOG",
            //    Icon = Device.OnPlatform<string>("tab_about.png", null, null)
            //});
         //   var list = new Playlist();
          //  var playitem = new PlaylistItem(list);
           // PlaylistPage page = new PlaylistPage(playitem);
            //Children.Add(new MyMusicPage()
            //{
            //    Title = "MY MUSIC",
            //    Icon = Device.OnPlatform<string>("tab_about.png", null, null)

            //});
            
     



        }
 
            
    }
}
