using Android.Content;
using Android.Preferences;
using Javax.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Droid.FileEncryption
{
   public class PrefUtils
    {
        public static  PrefUtils prefUtils = new PrefUtils();
        public static ISharedPreferences myPrefs = null;

        public static PrefUtils getInstance(Context context)
        {
            if (null == myPrefs)
                myPrefs = PreferenceManager.GetDefaultSharedPreferences(context);
            return prefUtils;
        }

        //public void saveSecretKey(String value)
        //{
        //    SharedPreferences.Editor editor = myPrefs.Edit();
        //    editor.putString(SecretKey, value);
        //    editor.commit();
        //}

        //public String getSecretKey()
        //{
        //    return myPrefs.GetString(SECRET_KEY, null);
        //}
    }
}
