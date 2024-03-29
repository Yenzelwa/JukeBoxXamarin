﻿[assembly: Xamarin.Forms.Dependency(typeof(JukeBox.Implementations.Config))]

namespace JukeBox.Implementations
{
    using JukeBox.Interfaces;

    public class Config : IConfig
    {
        private string directoryDB;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    directoryDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                }

                return directoryDB;
            }
        }

        //public ISQLitePlatform Platform
        //{
        //    get
        //    {
        //        if (platform == null)
        //        {
        //            platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
        //        }

        //        return platform;

        //    }
        //}
    }
}