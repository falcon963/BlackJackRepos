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
        void BotAdd();
        void Create(T item);
        void Delete(String nickname, String password);
        BotDAL GetBots(Int32 id);
        IEnumerable<String> GetAllPlayer();
        void DeleteAll();
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void SaveChangePlayer(PlayerDAL player, String nickname);
        void SaveChange(T baseEntities);
        PlayerDAL CheckAccountAccess(String nickname, String password);
        Boolean CheckValidNickname(String nickname);
        void UpdatePlayerAccount(T baseEntities);
    }
}
