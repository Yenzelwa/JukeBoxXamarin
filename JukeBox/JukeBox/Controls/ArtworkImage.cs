﻿using Android.Graphics;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JukeBox.Controls
{
    public class ArtworkImage : CachedImage
    {
        public static readonly BindableProperty ArtworkProperty = BindableProperty.Create<ArtworkImage, byte[]>(p => p.Artwork,null);

        public byte[] Artwork
        {
            
            get { return (byte[])GetValue(ArtworkProperty); }
            set
            {
                SetValue(ArtworkProperty, value);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch(propertyName)
            {
                case "Artwork":
                    System.Diagnostics.Debug.WriteLine("Artwork Changed Started");
                    System.Diagnostics.Debug.WriteLine("Artwork = " + Artwork);
                    if (!string.IsNullOrEmpty(Artwork?.ToString()) && !Artwork.ToString().Equals("False"))
                    {
                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            double width = WidthRequest, height = HeightRequest;
                            if (WidthRequest == -1)
                            {
                                System.Diagnostics.Debug.WriteLine("WidthRequest = -1");
                                width = 400;
                                height = 400;
                            } 
                            
                          //  System.IO.Stream stream = ((MPMediaItemArtwork)Artwork).ImageWithSize(new CoreGraphics.CGSize(width, height)).AsPNG().AsStream();
                           // Source = ImageSource.FromStream(() => stream);
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            Stream stream = new MemoryStream(Artwork);
                            Source = ImageSource.FromStream(() => { return stream; });
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Null");
                        Source = null;
                    }
                    System.Diagnostics.Debug.WriteLine("Artwork Finished");
                    break;
            }
        }
    }
}
