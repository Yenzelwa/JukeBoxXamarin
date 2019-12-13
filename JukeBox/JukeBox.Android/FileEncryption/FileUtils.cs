using Android.Content;
using Java.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JukeBox.Droid.FileEncryption
{
    public class FileUtils
    {

        public static void saveFile(byte[] encodedBytes, String path)
        {
            try
            {
                Java.IO.File file = new Java.IO.File(path);
                System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Open);
                BufferedOutputStream bos = new BufferedOutputStream(fs);
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
                //  e.PrintStackTrace();
            }

        }
        public static void saveDecFile(byte[] encodedBytes, String path , string fileName)
        {
            try
            {
                System.IO.File.Copy(path, "/storage/emulated/0/movies/"+ fileName);
                Java.IO.File file = new Java.IO.File(path);
                System.IO.FileStream fs = new System.IO.FileStream("/storage/emulated/0/movies/"+ fileName, FileMode.Open);
                BufferedOutputStream bos = new BufferedOutputStream(fs);
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
                //  e.PrintStackTrace();
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
        public static Java.IO.File createTempFile(string filename, byte[] decrypted)
        {
            Java.IO.File tempFile = new Java.IO.File("/storage/emulated/0/jukebox/Songs/decrepted"+filename );

            FileOutputStream fos = new FileOutputStream(tempFile);
            tempFile.DeleteOnExit();
            fos.Write(decrypted);
            fos.Close();
           
            return tempFile;
        }

        public static Java.IO.File getTempFileDescriptor2(string fileName, byte[] decrypted)
        {
            Java.IO.File tempFile = FileUtils.createTempFile(fileName, decrypted);
            FileInputStream fis = new FileInputStream(tempFile);
            return tempFile;
        }
        public static Java.IO.File getTempFileDescriptor(string fileName, byte[] decrypted)
        {
            Java.IO.File tempFile = FileUtils.createTempFile(fileName, decrypted);
            FileInputStream fis = new FileInputStream(tempFile);
             return tempFile;
    }

    public static String getDirPath(Context context)
        {
            return context.GetDir("DIR", FileCreationMode.Private).Path;
        }

        public static String getFilePath(Context context)
        {
            return getDirPath(context) + Java.IO.File.Separator + "test";
        }

        public static void deleteDownloadedFile(Context context)
        {
            Java.IO.File file = new Java.IO.File(getFilePath(context));
            if (null != file & file.Exists())
            {
                if (file.Delete()) ;
            }
        }
    }
}

    

