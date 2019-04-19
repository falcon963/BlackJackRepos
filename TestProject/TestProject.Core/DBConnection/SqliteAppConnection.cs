using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestProject.Core.Interfacies;
using TestProject.Core.Models;

namespace TestProject.Core.DBConnection
{
    public class SqliteAppConnection
    {
        public SQLiteConnection Database { get; set; }

        public SqliteAppConnection(IDatabaseConnectionService connectionService)
        {
            Database = connectionService.DbConnection();
            Database.CreateTable<UserTask>();
            Database.CreateTable<User>();
            Database.CreateTable<TaskFileModel>();
        }
    }
}
