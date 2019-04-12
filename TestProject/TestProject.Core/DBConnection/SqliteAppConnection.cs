using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;

namespace TestProject.Core.DBConnection
{
    public class SqliteAppConnection
    {
        private SQLiteConnection _database;

        public SQLiteConnection Database
        {
            get
            {
                return _database;
            }
            set
            {
                _database = value;
            }
        }

        public SqliteAppConnection(IDatabaseConnectionService connectionService)
        {
            Database = connectionService.DbConnection();
            Database.CreateTable<UserTask>();
            Database.CreateTable<User>();
            Database.CreateTable<TaskFileModel>();
        }
    }
}
