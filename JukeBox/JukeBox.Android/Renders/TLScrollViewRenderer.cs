using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JukeBox.RenderedViews;
using Xamarin.Forms.Platform.Android;

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