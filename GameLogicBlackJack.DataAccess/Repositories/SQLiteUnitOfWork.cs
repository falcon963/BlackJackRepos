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
    public class SQLiteUnitOfWork
    {
        private BlackJackContext context;
        private Repositories repositories;


        public SQLiteUnitOfWork(String namePath)
        {
            this.context = new BlackJackContext(namePath);
        } 

        public IRepository<BaseEntities> Enteties
        {
            get
            {
                if(repositories == null)
                {
                    repositories = new Repositories(context);
                }
                return repositories;
            }
        }
        
    }
}
