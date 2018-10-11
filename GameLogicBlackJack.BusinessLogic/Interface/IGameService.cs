using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Models;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;

namespace GameLogicBlackJack.BusinessLogic.Interface
{
    public interface IGameService
    {
        void BotAdd(Int32 num);
        void PlayerSave();
        void BotsSave();
        void DealerSave();
        void GameSave();
        String HashPassword(String password);
        Boolean VerifyHashedPassword(String login, String password);
        Boolean CheckLogin(String input);
        List<String> GetListPlayers();
        Boolean DeletePlayer(String login, String password);
        Int32 CheckBalance();
        Boolean CheckPlayerStatus();
        void GameDeal();
        void PlayerHitGame();
        void PlayerStandGame();
        List<Card> GetPlayerCards();
        List<Card> GetDealerCards();
        List<List<Card>> GetBotsCards();
        Int32 TotalValue(List<Card> hand);
        Boolean CheckPlayerWon();
        Boolean CheckPlayerLose();
        Boolean CheckPlayerDraw();
        void ClearDataBase();
    }
}
