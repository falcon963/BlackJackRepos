using SQLite;
using System.IO;
using TestProject.Core.Models;
using TestProject.Core.DBConnection.Interfacies;

namespace TestProject.Droid.Providers
{
    public class DatabaseConnectionProvider
        : IDatabaseConnectionService
    {
        public SQLiteConnection GetDBConnection(string dbName)
        {
            var path = Path.Combine(System.Environment.
              GetFolderPath(System.Environment.
              SpecialFolder.Personal), $"{dbName}.db3");

            return new SQLiteConnection(path);
        }
    }
}