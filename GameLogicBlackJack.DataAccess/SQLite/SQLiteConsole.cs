using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using GameLogicBlackJack.DataAccess.Interfaces;
using System.IO;

namespace GameLogicBlackJack.DataAccess.SQLite
{
    public class SQLiteConsole : ISQLite
    {
        public SQLiteConsole() { }

        public String GetDatabasePath(String sqliteFilename)
        {
            String documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }
}
