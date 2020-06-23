using JukeBox.RenderedViews;

namespace JukeBox.Droid.Renders
{
    public class TLScrollViewRenderer : Xamarin.Forms.Platform.Android.ScrollViewRenderer
    {
        public TLScrollViewRenderer(Android.Content.Context context) : base(context)
        {

        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as TLScrollView;
            element?.Render();
        }
    }
}