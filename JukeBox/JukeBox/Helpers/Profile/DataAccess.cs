namespace JukeBox.Helpers
{
    using Interfaces;
    using JukeBox.Models.Profile;
    using Models;
    using Plugin.Permissions;
    using SQLiteNetExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Xamarin.Forms;

    public class DataAccess : IDisposable
    {
        private SQLite.SQLiteConnection connection;

        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
      
            this.connection = new SQLite.SQLiteConnection(
                databasePath: Path.Combine(config.DirectoryDB, "JukeBox.db3") );
            connection.CreateTable<UserLocal>();
            connection.CreateTable<TokenResponse>();
        }

        public void Insert<T>(T model)
        {
            this.connection.Insert(model);
        }

        public void Update<T>(T model)
        {
            this.connection.Update(model);
        }

        public void Delete<T>(T model)
        {
            this.connection.Delete(model);
        }

        //public T First<T>(bool WithChildren) where T : class
        //{
        //    if (WithChildren)
        //    {
        //        //return connection.Table<T>().FirstOrDefault();
        //       return connection.GetAllWithChildren<T>().FirstOrDefault();
        //    }
        //    else
        //    {
        //        return connection.Table<T>().FirstOrDefault();
        //    }
        //}

        public IEnumerable<TokenResponse> GetMembers()
         {
            var members = (from mem in connection.Table<TokenResponse>() select mem);
            return members.ToList();
        }
        public IEnumerable<UserLocal> GetLocalMembers()
        {
            var members = (from mem in connection.Table<UserLocal>() select mem);
            return members.ToList();
        }

        //public T Find<T>(int pk, bool WithChildren) where T : class
        //{
        //    if (WithChildren)
        //    {
        //      //  return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
        //       return connection.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
        //    }
        //    else
        //    {
        //        return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
        //    }
        //}

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}