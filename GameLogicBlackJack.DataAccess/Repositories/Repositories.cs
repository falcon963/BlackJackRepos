using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;
using SQLite;

namespace GameLogicBlackJack.DataAccess.Repositories
{
    public class Repositories : IRepository<BaseEntities>
    {
        private SQLiteConnection database;

        public Repositories() { }

        public Repositories(String filename)
        {
            SQLiteConsole console = new SQLiteConsole();
            String databasePath = console.GetDatabasePath(filename);
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            database = new SQLiteConnection(databasePath);
            database.CreateTable<PlayerDAL>();
            database.CreateTable<BotDAL>();
            database.CreateTable<DealerDAL>();
            database.CreateTable<GameDAL>();
            database.CreateTable<BotSaves>();
        }

        public void BotAdd()
        {
           var bot = database.Insert(new BotDAL { Id = 1, Name = "Jim", Balance = 500, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 2, Name = "Fill", Balance = 600, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 3, Name = "Sam", Balance = 700, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 4, Name = "Bill", Balance = 560, Bet = 20 });
            bot = database.Insert(new BotDAL { Id = 5, Name = "Joker", Balance = 580, Bet = 20 });
        }


        public PlayerDAL CheckAccountAccess(String nickname, String password)
        {
            var pass = database.Table<PlayerDAL>().Where(v => v.Name == nickname).Select(v => v.Password).FirstOrDefault();
            return (pass == password) ? database.Get<PlayerDAL>(database.Table<PlayerDAL>().Where(v => v.Name == nickname).Select(v => v.Id).FirstOrDefault()) : null;
        }

        public Boolean CheckValidNickname(String nickname)
        {
            
            var login = database.Table<PlayerDAL>().Where(v => v.Name == nickname).FirstOrDefault();

            return login == null ? true : false;
            
        }


        public IEnumerable<String> GetAllPlayer()
        {
            return database.Table<PlayerDAL>().Select(s => s.Name);
        }

        public BotDAL GetBots(Int32 id)
        {
            return database.Get<BotDAL>(id);
        }

        public void Create(BaseEntities player)
        {
            database.Insert(player);
        }

        public void Delete(String nickname, String password)
        {
            var pass = database.Table<PlayerDAL>().Where(v => v.Name.Equals(nickname)).Select(p => p.Password).ToString();
            if(password == pass)
            {
               var player = database.Get<PlayerDAL>(database.Table<PlayerDAL>().Where(v => v.Name.Equals(nickname)).Select(p => p.Id).ToString());
               database.Delete(player);
            }
            if(password != pass)
            {
                new Exception("Denied access");
            }
        }

        public IEnumerable<BaseEntities> Find(Func<BaseEntities, Boolean> predicate)
        {
            return database.Table<BaseEntities>().Where(predicate).ToList();
        }

        public void SaveChangePlayer(PlayerDAL player, String nickname)
        {
            if (CheckValidNickname(nickname))
            {
                database.Insert(player);
            }
            if (!CheckValidNickname(nickname))
            {
                database.Update(player.Balance);
            }
        }

        public void SaveChange(BaseEntities baseEntities)
        {
                database.Insert(baseEntities);
        }

        public void DeleteAll()
        {
            Int32 count = database.Table<PlayerDAL>().Count();
            for (Int32 i = 1; i <= count; i++)
            {
                var player = database.Get<PlayerDAL>(i);
                if (player != null)

                    database.Delete(player);
            }
        }

        public void UpdatePlayerAccount(BaseEntities baseEntities)
        {
            database.Update(baseEntities);
        }

    }
}
