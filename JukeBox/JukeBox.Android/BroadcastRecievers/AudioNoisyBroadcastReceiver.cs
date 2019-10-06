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
using Android.Media;
using JukeBox.Audio;
using JukeBox.Droid.Audio;

namespace JukeBox.BroadcastRecievers
{
    [BroadcastReceiver]
    [IntentFilter(new[] { AudioManager.ActionAudioBecomingNoisy })]
    public class AudioNoisyBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != AudioManager.ActionAudioBecomingNoisy)
                return;

            Intent stopIntent = new Intent(AudioService.ActionPause);
            context.StartService(stopIntent);
        }
    }
}