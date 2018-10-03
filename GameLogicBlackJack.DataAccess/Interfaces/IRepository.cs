using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.DataAccess.Entities;

namespace GameLogicBlackJack.DataAccess.Interfaces
{
    public interface IRepository<T> where T: BaseEntities
    {
        void Create(T item);
        void Delete(String nickname, String password);
        IEnumerable<String> GetAllPlayer();
        void DeleteAll();
        void SaveChangePlayer(PlayerDAL player, String nickname);
        void SaveChangeGame(GameDAL game);
        void SaveChangeBot(BotDAL bot);
        void SaveChangeDealer(DealerDAL dealer);
        PlayerDAL CheckAccountAccess(String nickname, String password);
        Boolean CheckValidNickname(String nickname);
        void UpdatePlayerAccount(T baseEntities);
    }
}
