using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;
using System.IO;
using SQLite;
using SQLitePCL;

namespace GameLogicBlackJack.DataAccess.SQLite
{
    public class BlackJackContext
    {
        protected String namePath = "BlackJack.db3";

        public BlackJackContext(String namePath)
        {
            this.namePath = namePath;
        } 

        public void DataBaseCreateTable()
        {
            DataBaseConnection().CreateTables<PlayerDAL, BotDAL, DealerDAL, GameDAL>();
        }

        public SQLiteConnection DataBaseConnection()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), namePath);
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            return new SQLiteConnection(databasePath);
        }

        public void BotAdd()
        {
            var database = DataBaseConnection();
            var bot = database.Insert(new BotDAL {Id = 1, Name = "Jim", Balance = 500, Bet = 20});
            bot = database.Insert(new BotDAL { Id = 2, Name = "Fill", Balance = 600, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 3, Name = "Sam", Balance = 700, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 4, Name = "Bill", Balance = 560, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 5, Name = "Joker", Balance = 580, Bet = 20 });
            DataBaseConnection().CreateTable<BotDAL>();
            Console.WriteLine("Reading data");
            var table = database.Table<BotDAL>();
            foreach (var s in table)
            {
                Console.WriteLine(s.Id + " " + s.Name + " " + s.Balance + " " + s.Bet);
            }
        }


    }
}
