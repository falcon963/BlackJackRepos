using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Interfaces
{
    public interface IDatabaseConnectionService
    {
        SQLite.SQLiteConnection DbConnection();
    }
}
