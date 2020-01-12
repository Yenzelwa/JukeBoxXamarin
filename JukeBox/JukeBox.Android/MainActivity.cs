using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using JukeBox.Audio;
using Android.Provider;
using Android;
using Android.Database;
using Plugin.DownloadManager;
using System.Linq;
using DLToolkit.Forms.Controls;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Graphics;
using CarouselView.FormsPlugin.Android;
using System.Collections.Generic;
using System.Reflection;
using FFImageLoading.Forms.Platform;
using JukeBox.Droid.Audio;
using Java.IO;
using System.IO;
using Javax.Crypto;
using Javax.Crypto.Spec;
using System.Text;
using System.Security.Cryptography;
using Android.Support.V4.Media.Session;
using System.Net;
using JukeBox.Droid.FileEncryption;

namespace JukeBox.Droid
{
    [Activity(Label = "JukeBox", Icon = "@drawable/icon", Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;

        public bool IsBound = false;
        public Intent AudioServiceIntent;
        public static AudioServiceBinder Binder;

        private AudioServiceConnection _connection;
        private int STORAGE_PERMISSION_CODE = 1;

        private NotificationManagerCompat notificationManager;
        long stopTime, startTime;
        private string sKey = "0123456789abcdef";//key，
        private string ivParameter = "1020304050607080";
        private MediaSessionCompat mediaSession;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Download("Songs", null);
            // Set Status Bar Color
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Color.Black);
            notificationManager = NotificationManagerCompat.From(this);


            mediaSession = new MediaSessionCompat(this, "tag");

            CarouselViewRenderer.Init();

            CachedImageRenderer.Init(true);
          //  RegisterReceiver(savedInstanceState, new IntentFilter("com.companyname.IncomingCall"));

            //// FFImageLoading
            //CachedImageRenderer.Init();

            //// AudioService setup
            Instance = this;
            System.Threading.Tasks.Task.Run(() =>
            {
                Intent AudioServiceIntent = new Intent(this, typeof(AudioService));
                this.StartService(AudioServiceIntent);
                _connection = new AudioServiceConnection(this);
                bool binded = BindService(AudioServiceIntent, _connection, Bind.AutoCreate);
            });


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //// FlowListView
            FlowListView.Init();

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted || ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) != Permission.Granted)
            {
                // We have permission, go ahead and use the camera.
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity: this, Manifest.Permission.ReadExternalStorage)
                    || ActivityCompat.ShouldShowRequestPermissionRationale(activity: this, Manifest.Permission.ReadPhoneState))
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage , Manifest.Permission.ReadPhoneState }, STORAGE_PERMISSION_CODE);
                }

                else
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage , Manifest.Permission.ReadPhoneState }, STORAGE_PERMISSION_CODE);

                }



            }
            else
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity: this, Manifest.Permission.ReadExternalStorage)
                    || ActivityCompat.ShouldShowRequestPermissionRationale(activity: this, Manifest.Permission.ReadPhoneState))
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage , Manifest.Permission.ReadExternalStorage }, STORAGE_PERMISSION_CODE);
                }
                else
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.ReadPhoneState }, STORAGE_PERMISSION_CODE);

                }
            }
            LoadApplication(new App());
        }

        public bool encrypt(string filename)
        {
            // updateUI("Encrypting file...");
            try
            {
                byte[] fileData = FileUtils.readFile("/storage/emulated/0/jukebox/"+filename);
                byte[] encodedBytes = EncryptDecryptUtils.encode( fileData);
                FileUtils.saveFile(encodedBytes,"/storage/emulated/0/jukebox/"+filename);
                return true;
            }
            catch (Exception e)
            {
                // updateUI("File Encryption failed.\nException: " + e.getMessage());
            }
            return false;
        }
        public byte[] decrypt2(string filename)
        {
          //  updateUI("Decrypting file...");
            try
            {
                byte[] fileData = FileUtils.readFile("/storage/emulated/0/jukebox/" + filename);
                byte[] decryptedBytes = EncryptDecryptUtils.decode(fileData);
                return decryptedBytes;
            }
            catch (Exception e)
            {
               // updateUI("File Decryption failed.\nException: " + e.getMessage());
            }
            return null;
        }

        public void Download(string type, string typename)
        {

            string fileName ="", url = "";
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<Plugin.DownloadManager.Abstractions.IDownloadFile, string>
              (file =>
              {

                   fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                  url = file.Url;
                  Android.Net.Uri uri = MediaStore.Audio.Media.ExternalContentUri;
                  var subFolder = type == "Songs" ? type : $"Album/{typename}";
                  var dd = GetActualPathFromFile(uri);
                  var v = file.Status;
                  //  encrypt(fileName, file.Url);
                 //  decrypt(fileName);
                  //  EncryptFile(file.Url, $"/storage/emulated/0/jukebox/{subFolder}");
                  // var ddd = ApplicationContext.GetExternalFilesDir(dd).AbsolutePath;
                  return System.IO.Path.Combine($"/storage/emulated/0/jukebox/{subFolder}", fileName); 
                  
              });
           
        }

        public void encrypt(string filename , string path)
        {

            // Here you read the cleartext.
            try
            {
                var extStore = new Java.IO.File("/storage/emulated/0/jukebox/Songs");
                startTime = System.DateTime.Now.Millisecond;
                Android.Util.Log.Error("Encryption Started", extStore + "/" + filename);

                // This stream write the encrypted text. This stream will be wrapped by
                // another stream.
                createFile(filename, extStore);
                var webRequest = WebRequest.Create(path);

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent = reader.ReadToEnd();

                  //  System.IO.FileStream fs = System.IO.File.OpenWrite(path);
                    FileOutputStream fos = new FileOutputStream(extStore + "/" + filename + ".aes", false);

                    // Length is 16 byte
                    Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");
                    byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
                    SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
                    IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
                    cipher.Init(Javax.Crypto.CipherMode.EncryptMode, skeySpec, iv);

                    // Wrap the output stream
                    CipherInputStream cis = new CipherInputStream(content, cipher);
                    // Write bytes
                    int b;
                    byte[] d = new byte[1024 * 1024];
                    while ((b = cis.Read(d)) != -1)
                    {
                        fos.Write(d, 0, b);
                    }
                    // Flush and close streams.
                    fos.Flush();
                    fos.Close();
                    cis.Close();
                    stopTime = System.DateTime.Now.Millisecond;
                    Android.Util.Log.Error("Encryption Ended", extStore + "/5mbtest/" + filename + ".aes");
                    Android.Util.Log.Error("Time Elapsed", ((stopTime - startTime) / 1000.0) + "");
                }
            }
            catch (Exception e)
            {
                Android.Util.Log.Error("lv", e.Message);
            }

        }


        private void createFile(string filename, Java.IO.File extStore)
        {
            Java.IO.File file = new Java.IO.File(extStore + "/" + filename + ".aes");

            if (filename.IndexOf(".") != -1)
            {
                try
                {
                    file.CreateNewFile();
                }
                catch (Java.IO.IOException e)
                {
                    // TODO Auto-generated catch block
                    Android.Util.Log.Error("lv", e.Message);
                }
                Android.Util.Log.Error("lv", "file created");
            }
            else
            {
                file.Mkdir();
                Android.Util.Log.Error("lv", "folder created");
            }

            file.Mkdirs();
        }
        public void decrypt(string filename)
        {
            try
            {

                Java.IO.File extStore = new Java.IO.File("/storage/emulated/0/jukebox/Songs");
                Android.Util.Log.Error("Decryption Started", extStore + "");
                FileInputStream fis = new FileInputStream(extStore + "/" + filename + ".aes");

                createFile(filename, extStore);
                FileOutputStream fos = new FileOutputStream(extStore + "/" + "decrypted" + filename, false);
                System.IO.FileStream fs = System.IO.File.OpenWrite(extStore + "/" + "decrypted" + filename);
                // Create cipher

                Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");
                byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
                SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
                IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
                cipher.Init(Javax.Crypto.CipherMode.DecryptMode, skeySpec, iv);

                startTime = System.DateTime.Now.Millisecond;
                CipherOutputStream cos = new CipherOutputStream(fs, cipher);
                Java.IO.File file = new Java.IO.File("/storage/emulated/0/jukebox/Songs" + "/" + filename);
                if (file.Delete())
                {
                    Android.Util.Log.Error("File Deteted", extStore + filename);
                }
                else
                {
                    Android.Util.Log.Error("File Doesn't exists", extStore + filename);
                }

                int b;
                byte[] d = new byte[1024 * 1024];
                while ((b = fis.Read(d)) != -1)
                {
                    cos.Write(d, 0, b);
                }

                stopTime = System.DateTime.Now.Millisecond;

                Android.Util.Log.Error("Decryption Ended", extStore + "/" + "decrypted" + filename);
                Android.Util.Log.Error("Time Elapsed", ((stopTime - startTime) / 1000.0) + "");

                cos.Flush();
                cos.Close();
                fis.Close();
            }
            catch (Exception e)
            {
                Android.Util.Log.Error("lv", e.Message);
            }
        }

        public static byte[] readFile(String filePath)
        {
            byte[] contents;
            Java.IO.File file = new Java.IO.File(filePath);
            int size = (int)file.Length();
            contents = new byte[size];
            try
            {
                FileStream inputStream = new FileStream(filePath, FileMode.OpenOrCreate);
                Java.IO.BufferedOutputStream bos = new Java.IO.BufferedOutputStream(inputStream);
                BufferedInputStream buf = new BufferedInputStream(inputStream);
                try
                {
                    buf.Read(contents);
                    buf.Close();
                }
                catch (Java.IO.IOException e)
                {
                    e.PrintStackTrace();
                }
            }
            catch (Java.IO.FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            return contents;
        }
        public static void saveFile(byte[] encodedBytes, String path)
        {
            try
            {
                Java.IO.File file = new Java.IO.File(path);
                FileStream inputStream = new FileStream(path, FileMode.OpenOrCreate);
                Java.IO.BufferedOutputStream bos = new Java.IO.BufferedOutputStream(inputStream);
                bos.Write(encodedBytes);
                bos.Flush();
                bos.Close();

            }
            catch (Java.IO.FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }
            catch (Exception e)
            {
               
            }

        }

        override
        public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == STORAGE_PERMISSION_CODE)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    Toast.MakeText(context: this, text: "Permission Granted", ToastLength.Short);
                    if (Build.VERSION.SdkInt > BuildVersionCodes.M)
                    {
                        if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted
                        || CheckSelfPermission(Manifest.Permission.RecordAudio) != Android.Content.PM.Permission.Granted
                        || CheckSelfPermission(Manifest.Permission.ReadPhoneState) != Android.Content.PM.Permission.Granted)
                        {
                            RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.RecordAudio , Manifest.Permission.ReadPhoneState }, 0);
                        }
                    }
                }
                else
                {
                    Toast.MakeText(context: this, text: "Permission Deniel", ToastLength.Short);
                   System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }
            }

            //PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private string GetActualPathFromFile(Android.Net.Uri uri)
        {
            bool isKitKat = Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat;

            if (isKitKat && DocumentsContract.IsDocumentUri(this, uri))
            {
                // ExternalStorageProvider
                if (isExternalStorageDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);

                    char[] chars = { ':' };
                    string[] split = docId.Split(chars);
                    string type = split[0];

                    if ("primary".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        return Android.OS.Environment.ExternalStorageDirectory + "/" + split[1];
                    }
                }
                // DownloadsProvider
                else if (isDownloadsDocument(uri))
                {
                    string id = DocumentsContract.GetDocumentId(uri);

                    Android.Net.Uri contentUri = ContentUris.WithAppendedId(
                                    Android.Net.Uri.Parse("content://downloads/public_downloads"), long.Parse(id));

                    //System.Diagnostics.Debug.WriteLine(contentUri.ToString());

                    return getDataColumn(this, contentUri, null, null);
                }
                // MediaProvider
                else if (isMediaDocument(uri))
                {
                    String docId = DocumentsContract.GetDocumentId(uri);

                    char[] chars = { ':' };
                    String[] split = docId.Split(chars);

                    String type = split[0];

                    Android.Net.Uri contentUri = null;
                    if ("image".Equals(type))
                    {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    }
                    else if ("video".Equals(type))
                    {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    }
                    else if ("audio".Equals(type))
                    {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    }

                    String selection = "_id=?";
                    String[] selectionArgs = new String[]
                    {
                split[1]
                    };

                    return getDataColumn(this, contentUri, selection, selectionArgs);
                }
            }
            // MediaStore (and general)
            else if ("content".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {

                // Return the remote address
                if (isGooglePhotosUri(uri))
                    return uri.LastPathSegment;

                return getDataColumn(this, uri, null, null);
            }
            // File
            else if ("file".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }

        public static String getDataColumn(Context context, Android.Net.Uri uri, String selection, String[] selectionArgs)
        {
            ICursor cursor = null;
            String column = "_data";
            String[] projection =
            {
        column
    };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }
            return null;
        }

        //Whether the Uri authority is ExternalStorageProvider.
        public static bool isExternalStorageDocument(Android.Net.Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        //Whether the Uri authority is DownloadsProvider.
        public static bool isDownloadsDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        //Whether the Uri authority is MediaProvider.
        public static bool isMediaDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }

        public static bool isGooglePhotosUri(Android.Net.Uri uri)
        {
            return "com.google.android.apps.photos.content".Equals(uri.Authority);
        }
        protected override void OnDestroy()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            base.OnDestroy();
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == 16908332)
            {
                // Get the index for the current page.
                int index = ((App)App.Current).MainPage.Navigation.NavigationStack.Count - 1;
                // Get the current page.
                var currentPage = ((App)App.Current).MainPage.Navigation.NavigationStack[index];
                // Programmatically press the hardware back button.
                if (currentPage.SendBackButtonPressed())
                {
                    return true;
                }
                else
                {
                    return base.OnOptionsItemSelected(item);
                }
            }
            else
            {
                return base.OnOptionsItemSelected(item);
            }
        }
    }
}