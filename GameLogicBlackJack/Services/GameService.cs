using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Models;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.Services
{
    public class GameService
    {
        public static IUnitOfWork Database { get; private set; }

        private GameService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

       

        public static Bot GetBot(Int32? id)
        {
            var bot = Database.Bots.Get(id.Value);
            return new Bot { Id = bot.Id, Name = bot.Name, Balance = bot.Balance, Bet = bot.Bet };
        }

        public static void BotsInitialize(Game game)
        {
            Int32 number;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out number);
            while (number < 0 || number > 5)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out number);
            }
            for(int i = 0; i <= number; i++)
            {
                game.bots.Add(GetBot(i+1));
            }
        }


        public static void PlayerSave(Game game)
        {
            PlayerDAL playerDAL = new PlayerDAL()
            {
                Name = game.Player.Name,
                Balance = game.Player.Balance,
                Bet = game.Player.Bet,
                PlayerWon = game.Player.PlayerWon,
                PlayerDraw = game.Player.PlayerDraw
            };
        }

        public static void BotsSave(Game game)
        {
            foreach (Bot bot in game.bots)
            {
                BotSaves botSaves = new BotSaves()
                {
                    Id = bot.Id,
                    BotWon = bot.BotWon,
                    BotDraw = bot.BotDraw
                };
            }
        }

        public static void DealerSave(Game game)
        {
            DealerDAL dealerDAL = new DealerDAL()
            {
                Id = game.Dealer.Id
            };
        }

        public static void GameSave(Game game)
        {
            foreach (Bot bot in game.bots)
            {
                GameDAL gameDAL = new GameDAL()
                {
                    Id = game.GameId,
                    PlayerId = game.Player.Id,
                    DealerId = game.Dealer.Id,
                    BotId = bot.Id
                };
            }
        }

    }
}
