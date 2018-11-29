using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using TestProject.Core.Interfaces;
using SQLite;
using System.IO;

namespace TestProject.iOS.Database
{
    public class DatabaseConnectionService
        : IDatabaseConnectionService
    {
        public DatabaseConnectionService()
        {
            var database = DbConnection();
        }

        public SQLiteAsyncConnection DbConnection()
        {
            var dbName = "TaskyDB.db3";
            var path = Path.Combine(System.Environment.
            GetFolderPath(System.Environment.
            SpecialFolder.Personal), dbName);
            return new SQLiteAsyncConnection(path);
        }
    }
}