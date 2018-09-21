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

        public Repositories(BlackJackContext context)
        {
            this.database = context.DataBaseConnection();
        }

        public IEnumerable<BaseEntities> GetAll()
        {
            return database.Table<BaseEntities>();
        }

        public BaseEntities Get(Int32 id)
        {
            return database.Table<BaseEntities>().ElementAt(id);
        }

        public void Create(BaseEntities player)
        {
            database.Insert(player);
        }

        public void Delete(Int32 id)
        {
            BaseEntities player = database.Table<BaseEntities>().ElementAt(id);
            if (player != null)
            {
                database.Delete(player);
            }
        }

        public IEnumerable<BaseEntities> Find(Func<BaseEntities, Boolean> predicate)
        {
            return database.Table<BaseEntities>().Where(predicate).ToList();
        }
    }
}
