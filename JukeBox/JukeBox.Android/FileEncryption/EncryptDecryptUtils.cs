using Android.Content;
using Android.Preferences;
using Android.Util;
using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;
using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Droid.FileEncryption
{
    

    //public static void init(Context context)
    //{
    //    myPrefs = PreferenceManager.GetDefaultSharedPreferences(context);
    //}
    class EncryptDecryptUtils
    {
        public static string PROVIDER = "BC";
        public static string KEY_SPEC_ALGORITHM = "AES";
        public static string CIPHER_ALGORITHM = "AES/CBC/PKCS5Padding";
        public static string SECRET_KEY = "SECRET_KEY";

        public static  int OUTPUT_KEY_LENGTH = 256;
        private static string sKey = "0123456789abcdef";//key，
        private static string ivParameter = "1020304050607080";

        // public static SharedPreferences myPrefs = null;
        public static EncryptDecryptUtils instance = null;
        private static PrefUtils prefUtils;

        public static EncryptDecryptUtils getInstance(Context context)
        {

            if (null == instance)
                instance = new EncryptDecryptUtils();

            if (null == prefUtils)
                prefUtils = PrefUtils.getInstance(context);

            return instance;
        }

        public static byte[] encode( byte[] fileData)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(sKey);
            SecretKeySpec skeySpec = new SecretKeySpec(data, 0, data.Length, KEY_SPEC_ALGORITHM);
            Cipher cipher = Cipher.GetInstance(CIPHER_ALGORITHM, PROVIDER);
            IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));
            cipher.Init(Javax.Crypto.CipherMode.EncryptMode, skeySpec, iv);
            return cipher.DoFinal(fileData);
        }

        public static byte[] decode( byte[] fileData)
        {
            byte[]
            decrypted;
            Cipher cipher = Cipher.GetInstance(CIPHER_ALGORITHM, PROVIDER);
            byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
            SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
            IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
            cipher.Init(Javax.Crypto.CipherMode.DecryptMode, skeySpec, iv);
            decrypted = cipher.DoFinal(fileData);
            return decrypted;
        }

        //public void saveSecretKey(SecretKey secretKey)
        //{
        //    String encodedKey = Base64.EncodeToString(secretKey.getEncoded(), Base64.NoWrap);
        //    prefUtils.saveSecretKey(encodedKey);
        //}

        //public SecretKey getSecretKey()
        //{
        //    String encodedKey = prefUtils.getSecretKey();
        //    if (null == encodedKey)
        //    {
        //        SecureRandom secureRandom = new SecureRandom();
        //        KeyGenerator keyGenerator = null;
        //        try
        //        {
        //            keyGenerator = KeyGenerator.GetInstance(KEY_SPEC_ALGORITHM);
        //        }
        //        catch (NoSuchAlgorithmException e)
        //        {
        //            e.PrintStackTrace();
        //        }
        //        keyGenerator.Init(OUTPUT_KEY_LENGTH, secureRandom);
        //        SecretKey secretKey = keyGenerator.GenerateKey();
        //        saveSecretKey(secretKey);
        //        return secretKey;
        //    }

        //    byte[] decodedKey = Base64.Decode(encodedKey, Base64.NoWrap);
        //    var originalKey = new SecretKeySpec(decodedKey, 0, decodedKey.Length, KEY_SPEC_ALGORITHM);
        //    return originalKey;
        //}

    }
}
