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
using GameLogicBlackJack.DataAccess.Repositories;
using System.Security.Cryptography;
using GameLogicBlackJack.Controllers;

namespace GameLogicBlackJack.Services
{
    public class GameService
    {
        public SQLiteUnitOfWork Database { get; private set; }


        private GameService(SQLiteUnitOfWork unitOfWork)
        {
           Database = unitOfWork;
        }

        private static GameService _instance;
        
        public static GameService GetInstance(SQLiteUnitOfWork unit)
        {
            if(_instance == null)
            {
                _instance = new GameService(unit);
            }
            return _instance;
        }
        
        public Bot GetBot(Int32 id)
        {
            var bot = Database.Entities.GetBots(id);
            return new Bot { Id = bot.Id, Name = bot.Name, Balance = bot.Balance, Bet = bot.Bet };
        }

        public void BotsInitialize(Game game)
        {
            Console.WriteLine("Enter num bots");
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
                game.bots.Add(GetBot(i));
            }
        }

        public void BotAdd()
        {
            Database.Entities.BotAdd();
        }


        public void PlayerSave(Game game)
        {
            PlayerDAL playerDAL = new PlayerDAL()
            {
                Name = game.Player.Name,
                Balance = game.Player.Balance,
                Password = game.Player.Password
            };
            Database.Entities.SaveChangePlayer(playerDAL, game.Player.Name);
        }

        public void BotsSave(Game game)
        {
            foreach (Bot bot in game.bots)
            {
                BotSaves botSaves = new BotSaves()
                {
                    Id = bot.Id,
                    BotWon = bot.BotWon,
                    BotDraw = bot.BotDraw,
                    GameId = game.GameId
                };
                Database.Entities.SaveChange(botSaves);
            }
        }

        public void DealerSave(Game game)
        {
            DealerDAL dealerDAL = new DealerDAL()
            {
                Id = game.Dealer.Id
            };
            Database.Entities.SaveChange(dealerDAL);
        }

        public void GameSave(Game game)
        {

                GameDAL gameDAL = new GameDAL()
                {
                    Id = game.GameId,
                    PlayerId = game.Player.Id,
                    DealerId = game.Dealer.Id,
                    Bet = game.Bet,
                    PlayerWon = game.PlayerWon,
                    PlayerDraw = game.PlayerDraw,
                };
            Database.Entities.SaveChange(gameDAL);
        }

        public Int32  TakeNumber()
        {
            Int32 number;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out number);
            while (number <= 0)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out number);
            }
            return number;
        }

        public String HashPassword(String password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public Player VerifyHashedPassword()
        {
            var account = Database.Entities.CheckAccountAccess(ConsolePlayerNickname(), HashPassword(ConsolePlayerPassword()));

            return account != null ? new Player { Id = account.Id, Name = account.Name, Balance = account.Balance, Password = account.Password } : null;
        }

        public String ConsolePlayerPassword()
        {
            Console.WriteLine("Password");
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while (string.IsNullOrEmpty(inputLine) || inputLine.Contains(" "))
            {
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public String ConsolePlayerNewNickname()
        {
            Console.WriteLine("Nickname");
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while ((string.IsNullOrEmpty(inputLine) || inputLine.Contains(" ")) || !Database.Entities.CheckValidNickname(inputLine))
            {
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public String ConsolePlayerNickname()
        {
            Console.WriteLine("Nickname");
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while (string.IsNullOrEmpty(inputLine) || inputLine.Contains(" "))
            {
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public void Launcher(GameController controller)
        {
            
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.NumPad1)
                {
                    var account = VerifyHashedPassword();//загрузить существующий аккаунт
                    if (account != null)
                    {
                        controller.game.Player.Name = account.Name;
                        controller.game.Player.Balance = account.Balance;
                        controller.game.Player.Password = account.Password;
                        controller.game.Player.Id = account.Id;
                        BotsInitialize(controller.game);
                        controller.ConsoleChoise();
                    }
                }
                if (key.Key == ConsoleKey.NumPad2)
                {
                    var players = Database.Entities.GetAllPlayer();//получить всех играков +
                    foreach (String player in players)
                    {
                        Console.WriteLine(player);
                    }
                }
                if (key.Key == ConsoleKey.NumPad3)
                {
                    Database.Entities.Delete(ConsolePlayerNickname(), HashPassword(ConsolePlayerPassword()));//Удалить аккаунт
                }
                if (key.Key == ConsoleKey.NumPad4)//new account
                {
                    controller.NewGameInitialize();
                    controller.ConsoleChoise();
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }


        public Int32 ConsolePlayerBalance()
        {
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
            return balance;
        }

        public Int32 ConsolePlayerBet(Game game)
        {
            Int32 bet;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out bet);
            while (bet <= 0 || bet > game.Player.Balance)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out bet);
            }
            return bet;
        }
        public void DeleteAll()
        {
            Database.Entities.DeleteAll();
        }

    }
}
