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
using Android.Graphics;

using JukeBox;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using JukeBox.Effects;

[assembly: ResolutionGroupName("JukeBox")]
[assembly: ExportEffect(typeof(BlueSliderEffect), "BlueSliderEffect")]
namespace JukeBox.Effects
{
    class BlueSliderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var seekBar = (SeekBar)Control;
            seekBar.ProgressDrawable.SetColorFilter(new PorterDuffColorFilter(new Android.Graphics.Color(34, 135, 202), PorterDuff.Mode.SrcIn));
            seekBar.Thumb.SetColorFilter(new PorterDuffColorFilter(new Android.Graphics.Color(34, 135, 202), PorterDuff.Mode.SrcIn));
        }

        protected override void OnDetached()
        {

        }
    }
}