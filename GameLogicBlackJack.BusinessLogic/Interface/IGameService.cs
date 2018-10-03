using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Models;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;

namespace GameLogicBlackJack.BusinessLogic.Interface
{
    public interface IGameService
    {
        Bot GetBot(Int32? id);
        void BotsInitialize(Game game);
        void PlayerSave(Game game);
        void BotsSave(Game game);
        void DealerSave(Game game);
        void GameSave(Game game);
    }
}
