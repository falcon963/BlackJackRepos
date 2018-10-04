using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.ViewModels;
using GameLogicBlackJack.BusinessLogic.Models;
using GameLogicBlackJack.BusinessLogic.Services;

namespace GameLogicBlackJack.Controllers
{
    public class HomeController
    {
        GameService service;
        GameViewModel gameView = new GameViewModel();
        public HomeController()
        {
            service =  new GameService(); 
        }

        public Int32 TakeNumber()
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

        public void TakePlayerCardDeck()
        {
            var cards = service.GetPlayerCards();
            gameView.Player.hand = cards;
        }

        public void TakeDealerCardDeck()
        {
            var cards = service.GetDealerCards();
            gameView.Dealer.hand = cards;
        }

        public void TakeBotCardDeck()
        {
            var cards = service.GetBotsCards();
            foreach(List<Card> bot in cards)
            {
                Int32 i = bot.Count;
                gameView.bots[i - 1].hand = bot;
                if(i - 1 == 0)
                {
                    break;
                }
            }
        }

        public void DrawPlayerCard()
        {
            TakePlayerCardDeck();
            Console.ForegroundColor = ConsoleColor.Green;
            Int32 countCard = gameView.Player.hand.Count();
            Console.WriteLine("-------------------------------------------------------------------\n");
            for(Int32 i = 0; i < countCard; i++)
            {
                Console.WriteLine("{0} card" + (i + 1) + ": {1}\n", gameView.Player.Name, gameView.Player.hand[i].CardFace + " " + gameView.Player.hand[i].CardSuit);
            }
            Console.WriteLine("{0} score: {1}\n", gameView.Player.Name, service.TotalValue(gameView.Player.hand));
            Console.WriteLine("-------------------------------------------------------------------\n");
        }

        public void DrawBotsCard()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Bot bot in service.game.bots)
            {
                Int32 countCard = bot.hand.Count();
                for (Int32 i = 0; i < countCard; i++)
                {
                    Console.WriteLine("{0} card" + (i + 1) + ": {1}\n", bot.Name, bot.hand[i].CardFace + " " + bot.hand[i].CardSuit);
                }
                Console.WriteLine("{0} score: {1}\n", bot.Name, service.TotalValue(bot.hand));
            }
        }

        public void DrawDealerCard()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Int32 countCard = service.game.Dealer.hand.Count();
            for (Int32 i = 0; i < countCard; i++)
            {
                Console.WriteLine("Dealer card" + (i + 1) + ": {0}\n", service.game.Dealer.hand[i].CardFace + " " + service.game.Dealer.hand[i].CardSuit);
            }
            Console.WriteLine("Dealer score: {0}\n", service.TotalValue(service.game.Dealer.hand));
        }

        public String ConsolePlayerPassword()
        {
            Console.WriteLine("Enter you password:");
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while (string.IsNullOrEmpty(inputLine) || inputLine.Contains(" "))
            {
                Console.WriteLine("Enter you password:");
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public String ConsolePlayerNewNickname()
        {
            Console.WriteLine("Enter you nickname:");
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while ((string.IsNullOrEmpty(inputLine) || inputLine.Contains(" ")) || !service.CheckLogin(inputLine))
            {
                Console.WriteLine("Enter you nickname:");
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public String ConsolePlayerNickname()
        {
            Console.WriteLine("Enter you nickname:");
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while (string.IsNullOrEmpty(inputLine) || inputLine.Contains(" "))
            {
                Console.WriteLine("Enter you nickname:");
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public void Launcher()
        {

            while (true)
            {
                Console.WriteLine("Menu:\n1|\tLoad\n2|\tPlayers list\n3|\tDelete account\n4|\tNew account\nEsc|\tExit");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.NumPad1)
                {
                    try
                    {
                        var account = service.VerifyHashedPassword(ConsolePlayerNickname(), ConsolePlayerPassword());//загрузить существующий аккаунт
                        if (account != null)
                        {
                            gameView.Player.Name = account.Name;
                            gameView.Player.Balance = account.Balance;
                            gameView.Player.Password = account.Password;
                            gameView.Player.Id = account.Id;
                            ConvertInPlayer();
                            BotsInitialize();
                            GameStart();
                        }
                    }
                    catch (InvalidCastException)
                    {
                        Console.WriteLine("Wrong login or password!");
                    }
                }
                if (key.Key == ConsoleKey.NumPad2)
                {
                    var players = service.GetListPlayers();//получить всех играков +
                    foreach (String player in players)
                    {
                        Console.WriteLine(player);
                    }
                }
                if (key.Key == ConsoleKey.NumPad3)
                {
                    try
                    {
                        service.DeletePlayer(ConsolePlayerNickname(), ConsolePlayerPassword());//Удалить аккаунт
                    }
                    catch (InvalidCastException)
                    {
                        Console.WriteLine("Wrong login or password!");
                    }
                }
                if (key.Key == ConsoleKey.NumPad4)//new account
                {
                    NewGameInitialize();
                    GameStart();
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Exit complite.");
                    break;
                }
            }
        }

        public void GameStart()
        {
            try
            {
                if (service.CheckBalance() <= 0)
                {
                    throw new ArgumentNullException();
                }
                gameView.Bet = ConsolePlayerBet();
                ConvertInGame();
                service.Deal();
                DrawPlayerCard();
                DrawBotsCard();
                DrawDealerCard();
                OutPlayerStatus();
                while (!service.CheckPlayerStatus())
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Enter if you want continue, or press Space if you want take card\n");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        service.Stand();
                        DrawDealerCard();
                        break;
                    }
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        service.Hit();
                        DrawPlayerCard();
                    }
                }
                DrawBotsCard();
                CheckBotStatus();
                OutPlayerStatus();
                service.DealerSave();
                service.GameSave();
                service.BotsSave();
                Console.WriteLine("{0} | {1}", service.game.Player.Name, service.game.Player.Balance);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Insufficient funds in the account!");
                gameView.Player.Balance = ConsolePlayerBalance();
                ConvertInPlayer();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
                service.PlayerSave();
                Console.WriteLine("Start new game - press N, stop game - press Esc\n");
                ConsoleKeyInfo keyNewGame = Console.ReadKey(true);
                if (keyNewGame.Key == ConsoleKey.N)
                {
                    GameStart();
                }
                if (keyNewGame.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Game was stop");
                }
            }
        }


        public void OutPlayerStatus()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (service.CheckPlayerWon())
            {
                Console.WriteLine("{0}, won!", gameView.Player.Name);
            }
            if (service.CheckPlayerLose())
            {
                Console.WriteLine("{0}, lose!", gameView.Player.Name);
            }
            if (service.CheckPlayerDraw())
            {
                Console.WriteLine("{0}, play a draw!", gameView.Player.Name);
            }
        }

        public void CheckBotStatus()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Bot bot in service.game.bots)
            {
                if (bot.BotState == BusinessLogic.Enums.BotState.BotWon)
                {
                    Console.WriteLine("{0} won!", bot.Name);
                }
                if (bot.BotState == BusinessLogic.Enums.BotState.BotLose)
                {
                    Console.WriteLine("{0} lose!", bot.Name);
                }
                if (bot.BotState == BusinessLogic.Enums.BotState.BotDraw)
                {
                    Console.WriteLine("{0} play a draw!", bot.Name);
                }
            }
        }

        public void NewGameInitialize()
        {
            gameView.Player.Name = ConsolePlayerNewNickname();
            gameView.Player.Password = service.HashPassword(ConsolePlayerPassword());
            gameView.Player.Balance = ConsolePlayerBalance();
            ConvertInPlayer();
            Console.WriteLine(service.CheckBalance());
            BotsInitialize();
        }

        


        public Int32 ConsolePlayerBalance()
        {
            Console.WriteLine("Enter balance:");
            Int32 balance;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out balance);
            while (balance <= 0 || balance >= 1000)
            {
                Console.WriteLine("Enter balance:");
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out balance);
            }
            return balance;
        }

        public Int32 ConsolePlayerBet()
        {
            Console.WriteLine("Deal bet (1 - {0})", service.game.Player.Balance);
            Int32 bet;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out bet);
            while (bet <= 0 || bet > service.game.Player.Balance)
            {
                Console.WriteLine("Deal bet (1 - {0})", service.game.Player.Balance);
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out bet);
            }
            return bet;
        }


        public void ConvertInGame()
        {
            service.game.Bet = gameView.Bet;
        }

        public void ConvertInPlayer()
        {
            service.game.Player.Name = gameView.Player.Name;
            service.game.Player.Password = gameView.Player.Password;
            service.game.Player.Balance = gameView.Player.Balance;
        }

        public void ConvertInBot()
        {
            foreach (BotViewModel botView in gameView.bots)
            {
                Bot bot = new Bot()
                {
                    BotWon = botView.BotWon,
                    BotDraw = botView.BotDraw,
                    Balance = botView.Balance,
                    Bet = botView.Bet,
                    Name = botView.Name
                };
            }
        }

        public void ConvertInDealer()
        {
            Dealer dealer = new Dealer()
            {
                Id = gameView.Dealer.Id,
            };
        }

        public void BotsInitialize()
        {
            Console.WriteLine("Enter num bots:");
            Int32 number;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out number);
            while (number < 0 || number > 5)
            {
                Console.WriteLine("Enter num bots:");
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out number);
            }
            service.BotAdd(number);
        }
    }
}
