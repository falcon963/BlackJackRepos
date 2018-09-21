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
    public class GameService : IGameService
    {
        IUnitOfWork Database { get; set; }
        List<Bot> bots = new List<Bot>();

        public GameService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public Int32 TotalValue(List<Card> hand)
        {
            Int32 totalValue = 0;
            foreach (Card card in hand)
            {
                if (card.CardFace == CardFaceEnum.Ace & totalValue + 11 < 22)
                {
                    card.CardValue = 11;
                }
                if (card.CardFace == CardFaceEnum.Ace & totalValue + 11 > 22)
                {
                    card.CardValue = 1;
                }
                totalValue += card.CardValue;
            }
            return totalValue;
        }

        public Bot GetBot(Int32? id)
        {
            var bot = Database.Bots.Get(id.Value);
            return new Bot { Id = bot.Id, Name = bot.Name, Balance = bot.Balance, Bet = bot.Bet };
        }

        public void BotsInitialize()
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
            for(int i = 1; i <= number; i++)
            {
                bots.Add(GetBot(i));
            }
        }

        public void DealerInitialize()
        {
            DealerDAL dealerDAL = new DealerDAL();
        }

        public void PlayerInitialize()
        {
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while (string.IsNullOrEmpty(inputLine) || inputLine.Contains(" "))
            {
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            Int32 balance;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out balance);
            while (balance <= 0 || balance >= 1000)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out balance);
            }
            Int32 bet;
            input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out bet);
            while (bet <= 0 || bet > balance)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out bet);
            }
            PlayerDAL playerDAL = new PlayerDAL()
            {
                Name = inputLine,
                Balance = balance,
                Bet = bet
            };
        }

        public void GameInitialize(Game game)
        {
            PlayerDAL playerDAL = Database.Players.Get(game.PlayerId);
            BotDAL botDAL = Database.Bots.Get(game.BotId);
            DealerDAL dealerDAL = Database.Dealers.Get(game.DealerId);

            GameDAL gameDAL = new GameDAL()
            {
                PlayerId = playerDAL.Id,
                BotId = botDAL.Id,
                DealerId = dealerDAL.Id,
            };

        }
    }
}
