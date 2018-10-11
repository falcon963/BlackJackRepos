using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
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
            return context.Players.Where(v => v.Name == nickname).FirstOrDefault();
        }

        public Boolean CheckValidNickname(String nickname)
        {
            
            var login = context.Players.Where(v => v.Name == nickname).FirstOrDefault();

            return login == null ? true : false;
            
        }


        public IEnumerable<String> GetAllPlayers()
        {
            return context.Players.Select(s => s.Name);
        }




        public Boolean Delete(String nickname, String password)
        {
            var pass = context.Players.Where(v => v.Name == nickname).Select(p => p.Password).FirstOrDefault();
            if(password == pass)
            {
                var players = context.Players.ToList();
                var playerToDelete = players.Where(p => p.Name == nickname).FirstOrDefault();
                context.Players.Remove(playerToDelete);
                context.SaveChanges();
            }
            return (password == pass) ? true : false;
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
                var playerToUpdate = players.Select(p => p).Where(p => p.Id == player.Id).FirstOrDefault();
                playerToUpdate.Balance = player.Balance;
                context.Players.Update(playerToUpdate);
            }
            context.SaveChanges();
        }

        public Int32 ReturnPlayerId(String nickname)
        {
            var playerId = context.Players.Where(v => v.Name == nickname).Select(s => s.Id).FirstOrDefault();
            return playerId;
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

        public void UniversalSaveChanges(BaseEntities baseEntities)
        {
            if(baseEntities is GameDAL)
            {
                context.Add(baseEntities);
            }
            if(baseEntities is BotDAL)
            {
                context.Add(baseEntities);
            }
            if(baseEntities is DealerDAL)
            {
                context.Add(baseEntities);
            }
            context.SaveChanges();
        }

        public void SaveChangeDealer(DealerDAL dealer)
        {
            context.Dealers.Add(dealer);
            context.SaveChanges();
        }

        public void ClearDataBase()
        {
            context.RemoveRange(context.Players);
            context.RemoveRange(context.Games);
            context.RemoveRange(context.Bots);
            context.RemoveRange(context.Dealers);
            context.SaveChanges();
        }
    }
}
