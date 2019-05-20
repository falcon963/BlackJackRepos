using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Providers.Interfacies
{
    public interface IDatabaseConnectionProvider
    {
        SQLiteConnection GetDBConnection(string dbName);
    }
}
