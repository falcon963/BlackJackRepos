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
        public SQLiteConnection GetDBConnection(string dbName)
        {
            var path = Path.Combine(Environment.
            GetFolderPath(Environment.
            SpecialFolder.Personal), $"{dbName}.db3");
            return new SQLiteConnection(path);
        }
    }
}