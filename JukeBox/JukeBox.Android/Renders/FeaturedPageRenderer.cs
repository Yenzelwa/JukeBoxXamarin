using Android.Support.Design.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using JukeBox.Renders;
using JukeBox.Views;

[assembly: ExportRenderer(typeof(LandingTabbedPage), typeof(FeaturedPageRenderer))]
namespace JukeBox.Renders
{

#pragma warning disable CS0618 // Type or member is obsolete
    public class FeaturedPageRenderer : TabbedPageRenderer, TabLayout.IOnTabSelectedListener
        {
            private LandingTabbedPage _page;
            protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
            {
                base.OnElementChanged(e);
                if (e.NewElement != null)
                {
                    _page = (LandingTabbedPage)e.NewElement;
                }
                else
                {
                    _page = (LandingTabbedPage)e.OldElement;
                }

            }
            async void TabLayout.IOnTabSelectedListener.OnTabReselected(TabLayout.Tab tab)
            {
                await _page.CurrentPage.Navigation.PopToRootAsync();
            }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
