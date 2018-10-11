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
        Boolean Delete(String nickname, String password);
        IEnumerable<String> GetAllPlayers();
        void SaveChangePlayer(PlayerDAL player, String nickname);
        PlayerDAL CheckAccountAccess(String nickname, String password);
        Boolean CheckValidNickname(String nickname);
        void UniversalSaveChanges(BaseEntities baseEntities);
        void ClearDataBase();
        Int32 ReturnPlayerId(String nickname);
    }
}
