using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite.EF6;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GameLogicBlackJack.DataAccess.Entities;
using System.Data.Entity;
using System.IO;


namespace GameLogicBlackJack.DataAccess.Repositories
{
        public class BlackJackContext : DbContext
        {

        public DbSet<PlayerDAL> Players { get; set; }
        public DbSet<DealerDAL> Dealers { get; set; }
        public DbSet<BotDAL> Bots { get; set; }
        public DbSet<GameDAL> Games { get; set; }

        public BlackJackContext()
        {
            Database.EnsureCreated();
        }

        public static String ApplicationFolder(String filename = "")=>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename);




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var _databasePath = DatabaseFolder();
            optionsBuilder.UseSqlite(@"Filename={_databasePath}", x => x.SuppressForeignKeyEnforcement());
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public static String DatabaseFolder(String filename = "BlackJackDBEF.sqlite3")
        {
            return ApplicationFolder(filename);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        }

}
