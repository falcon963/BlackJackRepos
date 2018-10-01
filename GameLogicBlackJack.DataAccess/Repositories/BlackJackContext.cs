using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite.EF6;
using Microsoft.EntityFrameworkCore;
using GameLogicBlackJack.DataAccess.Entities;
using System.Data.Entity;
using System.IO;

namespace GameLogicBlackJack.DataAccess.Repositories
{
    namespace GameLogicBlackJack.DataAccess.SQLite
    {
        public class BlackJackContext : DbContext
        {
           
            public DbSet<PlayerDAL> Players { get; set; }
            public DbSet<DealerDAL> Dealers { get; set; }
            public DbSet<BotDAL> Bots { get; set; }
            public DbSet<BotSaves> BotsSaves { get; set; }
            public DbSet<GameDAL> Games { get; set; }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {

                optionsBuilder.UseSqlite(@"DataSource=blackJack.db;");
            }
        }
    }
}
