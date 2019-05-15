using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestProject.Core.DBConnection.Interfacies;
using TestProject.Core.Models;

namespace TestProject.Core.DBConnection
{
    public class SqliteAppConnection
    {
        public SQLiteConnection Database { get; set; }

        public SqliteAppConnection(IDatabaseConnectionService connectionService)
        {
            string dbName = "TaskyDrop";
            Database = connectionService.GetDBConnection(dbName);
            Database.CreateTable<UserTask>();
            Database.CreateTable<User>();
            Database.CreateTable<TaskFileModel>();
        }
    }
}
