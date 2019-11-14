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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Download("Songs", null);
            // Set Status Bar Color
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Color.Black);

            CarouselViewRenderer.Init();

            CachedImageRenderer.Init(true);

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

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                // We have permission, go ahead and use the camera.
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity: this, Manifest.Permission.ReadExternalStorage))
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, STORAGE_PERMISSION_CODE);
                }

                else
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, STORAGE_PERMISSION_CODE);

                }

            }
            else
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity: this, Manifest.Permission.ReadExternalStorage))
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, STORAGE_PERMISSION_CODE);
                }
                else
                {
                    ActivityCompat.RequestPermissions(activity: this, new String[] { Manifest.Permission.ReadExternalStorage }, STORAGE_PERMISSION_CODE);

                }
            }
            LoadApplication(new App());
        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
        public void Download(string type, string typename)
        {


            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<Plugin.DownloadManager.Abstractions.IDownloadFile, string>
                (file =>
                {

                    string fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                    Android.Net.Uri uri = MediaStore.Audio.Media.ExternalContentUri;
                    var subFolder = type == "Songs" ? type : $"Album/{typename}";
                    var dd = GetActualPathFromFile(uri);
                  //  EncryptFile(file.Url, $"/storage/emulated/0/jukebox/{subFolder}");
                    // var ddd = ApplicationContext.GetExternalFilesDir(dd).AbsolutePath;
                    return System.IO.Path.Combine($"/storage/emulated/0/jukebox/{subFolder}", fileName);
                });
        }

private void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = "65gyuguyu"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                
            }
        }




private void DecryptFile(string inputFile, string outputFile)
        {

            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

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
                        || CheckSelfPermission(Manifest.Permission.RecordAudio) != Android.Content.PM.Permission.Granted)
                        {
                            RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.RecordAudio }, 0);
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

        //Whether the Uri authority is Google Photos.
        public static bool isGooglePhotosUri(Android.Net.Uri uri)
        {
            return "com.google.android.apps.photos.content".Equals(uri.Authority);
        }
        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    if (requestCode == 0)
        //    {
        //        var uri = data.Data;
        //        string path = GetActualPathFromFile(uri);
        //        System.Diagnostics.Debug.WriteLine("Image path == " + path);

        //    }
        //}

        protected override void OnDestroy()
        {
            StartService(new Intent(JukeBox.Droid.Audio.AudioService.ActionTryKill));
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