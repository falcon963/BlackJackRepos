using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using SQLite;
using GameLogicBlackJack.DataAccess.Repositories;

namespace GameLogicBlackJack.DataAccess.Repositories
{
    public class SQLiteUnitOfWork : IUnitOfWork
    {
        private Repositories repositories;
        private BlackJackContext database;

        public SQLiteUnitOfWork()
        {
            database = new BlackJackContext();
        }

        public IRepository<BaseEntities> Entities
        {
            get
            {
                if(repositories == null)
                {
                    repositories = new Repositories(database);
                }
                return repositories;
            }
        }

    }
}
