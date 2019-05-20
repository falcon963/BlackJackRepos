using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestProject.Core.Providers.Interfacies;
using TestProject.Core.Models;

namespace TestProject.Core.Providers
{
    public class SqliteAppConnectionProvider
    {
        public SQLiteConnection Database { get; set; }

        public SqliteAppConnectionProvider(IDatabaseConnectionProvider connectionService)
        {
            string dbName = "TaskyDrop";
            Database = connectionService.GetDBConnection(dbName);
            Database.CreateTable<UserTask>();
            Database.CreateTable<User>();
            Database.CreateTable<TaskFileModel>();
        }
    }
}
