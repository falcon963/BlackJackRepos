using SQLite;
using LocalDataAccess.Droid;
using System.IO;
using TestProject.Core.Interfacies;
using TestProject.Core.Models;

namespace LocalDataAccess.Droid
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
            var path = Path.Combine(System.Environment.
              GetFolderPath(System.Environment.
              SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
    }
}