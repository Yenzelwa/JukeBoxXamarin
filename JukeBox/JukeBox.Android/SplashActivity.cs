﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Content.PM;
using JukeBox.Audio;
using Android.Graphics;
using DLToolkit.Forms.Controls;
using Android.Graphics.Drawables;
using JukeBox.Droid;

namespace JukeBox
{
    [Activity(Label = "JukeBox World", MainLauncher = true, NoHistory = true, Theme = "@style/MainTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            base.OnCreate(bundle);

            // FFImageLoading
           // CachedImageRenderer.Init();

            //// AudioService setup
            //AudioServiceIntent = new Intent(Droid.Audio.AudioService.ActionStart);
            //ComponentName name = StartService(AudioServiceIntent);
            //var _connection = new AudioServiceConnection(this);
            //bool binded = BindService(AudioServiceIntent, _connection, Bind.AutoCreate);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // FlowListView
            FlowListView.Init();

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}