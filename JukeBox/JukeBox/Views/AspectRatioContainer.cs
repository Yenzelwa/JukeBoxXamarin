using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace JukeBox.Views
{
    public class AspectRatioContainer : ContentView
    {
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint = this.AspectRation));
        }
        public static BindableProperty AspectRationProperty = BindableProperty.Create(nameof(AspectRation), typeof(double), typeof(AspectRatioContainer), (double)1);

        public double AspectRation
        {
            get { return (double)GetValue(AspectRationProperty); }
            set
            {
                this.SetValue(AspectRationProperty, value);
            }

        }

    }
}