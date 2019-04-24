using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using SQLite;
using System.IO;
using TestProject.Core.DBConnection.Interfacies;

namespace TestProject.iOS.Database
{
    public class DatabaseConnectionService
        : IDatabaseConnectionService
    {
        public DatabaseConnectionService()
        {
            var database = DbConnection();
        }

        public SQLiteConnection DbConnection()
        {
            var dbName = "TaskyDB.db3";
            var path = Path.Combine(Environment.
            GetFolderPath(Environment.
            SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
    }
}