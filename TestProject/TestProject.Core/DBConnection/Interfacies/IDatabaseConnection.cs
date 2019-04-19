using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.DBConnection.Interfacies
{
    public interface IDatabaseConnectionService
    {
        SQLite.SQLiteConnection DbConnection();
    }
}
