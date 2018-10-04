using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;
using SQLite;
using Microsoft.Data.Sqlite;

namespace GameLogicBlackJack.DataAccess.Repositories
{
    public class Repositories : IRepository<BaseEntities>
    {
        BlackJackContext context;
        public Repositories(BlackJackContext context)
        {
            this.context = context;
        }

        


        public PlayerDAL CheckAccountAccess(String nickname, String password)
        {
            var pass = context.Players.Where(v => v.Name == nickname).Select(v => v.Password).FirstOrDefault();
            if (password != pass)
            {
                throw new InvalidCastException();
            }
            return context.Players.Where(v => v.Name == nickname).FirstOrDefault();
        }

        public Boolean CheckValidNickname(String nickname)
        {
            
            var login = context.Players.Where(v => v.Name == nickname).FirstOrDefault();

            return login == null ? true : false;
            
        }


        public IEnumerable<String> GetAllPlayer()
        {
            return context.Players.Select(s => s.Name);
        }

        public void Create(BaseEntities player)
        {
            context.Add(player);
            context.SaveChanges();
        }


        public void Delete(String nickname, String password)
        {
            var pass = context.Players.Where(v => v.Name == nickname).Select(p => p.Password).FirstOrDefault();
            if(password == pass)
            {
                var players = context.Players.ToList();
                var playerToDelete = players.Where(p => p.Name == nickname).FirstOrDefault();
                context.Players.Remove(playerToDelete);
                context.SaveChanges();
            }
            if(password != pass)
            {
                throw new InvalidCastException();
            }
        }


        public void SaveChangePlayer(PlayerDAL player, String nickname)
        {
            if (CheckValidNickname(nickname))
            {
                context.Players.Add(player);
            }
            if (!CheckValidNickname(nickname))
            {
                var players = context.Players.ToList();
                var playerToUpdate = players.Select(p => p).Where(p => p.Name==player.Name).FirstOrDefault();
                playerToUpdate.Balance = player.Balance;
                context.Players.Update(playerToUpdate);
            }
            context.SaveChanges();
        }

        public void SaveChangeGame(GameDAL game)
        {
            context.Games.Add(game);
            context.SaveChanges();
        }

        public void SaveChangeBot(BotDAL bot)
        {
            context.Bots.Add(bot);
            context.SaveChanges();
        }

        public void SaveChangeDealer(DealerDAL dealer)
        {
            context.Dealers.Add(dealer);
            context.SaveChanges();
        }


        public void DeleteAll()
        {
            Int32 count = context.Players.Count();
            for (Int32 i = 1; i <= count; i++)
            {
                var player = context.Players.Find(i);
                if (player != null)

                    context.Remove(player);
                context.SaveChanges();
            }
        }

        public void UpdatePlayerAccount(BaseEntities baseEntities)
        {
            context.Update(baseEntities);
            context.SaveChanges();
        }

    }
}
