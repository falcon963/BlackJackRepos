using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Models;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;

namespace GameLogicBlackJack.Interface
{
    public interface IGameService
    {
        Int32 CardTotalValue(List<Card> Hand);
        Bot GetBot(Int32? id);
        void BotsInitialize();
        void PlayerInitialize();
        void DealerInitialize();
        void BotPlaying();
        void BotAndDealerPlaying(BotDAL bot, DealerDAL dealer);
        void PlayerChoise(Player player);
        void PlayerCheckupGoldBlackJack(Player player);
        void BotCheckupGoldBlackJack(Player player);
        void Deal(Player player, Dealer dealer, Bot bot);
        void Hit(Player player);
        void Stand(Player player, Dealer dealer, Bot bot);
    }
}
