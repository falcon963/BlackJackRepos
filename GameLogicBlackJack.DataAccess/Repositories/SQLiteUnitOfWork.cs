using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;
using SQLite;
using GameLogicBlackJack.DataAccess.Repositories;

namespace GameLogicBlackJack.DataAccess.Repositories
{
    public class SQLiteUnitOfWork : IUnitOfWork
    {
        private Repositories repositories;
        public const string DATABASE_NAME = "BlackJackDataBaseV1.2.db3";

        public SQLiteUnitOfWork() { }

        public IRepository<BaseEntities> Entities
        {
            get
            {
                if(repositories == null)
                {
                    repositories = new Repositories(DATABASE_NAME);
                }
                return repositories;
            }
        }

        
    }
}
