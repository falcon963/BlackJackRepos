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
        void BotAdd(Int32 num);
        void PlayerSave();
        void BotsSave();
        void DealerSave();
        void GameSave();
        String HashPassword(String password);
        Player VerifyHashedPassword(String login, String password);
        Boolean CheckLogin(String input);
        List<String> GetListPlayers();
        void DeletePlayer(String login, String password);
        Int32 CheckBalance();
        Boolean CheckPlayerStatus();
        void Deal();
        void Hit();
        void Stand();
        List<Card> GetPlayerCards();
        List<Card> GetDealerCards();
        List<List<Card>> GetBotsCards();
        Int32 TotalValue(List<Card> hand);
        Boolean CheckPlayerWon();
        Boolean CheckPlayerLose();
        Boolean CheckPlayerDraw();
        List<Bot> UpdateBotStatus();
    }
}
