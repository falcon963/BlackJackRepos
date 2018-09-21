using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Models;
using GameLogicBlackJack.View;

namespace GameLogicBlackJack.Controllers
{
    public class GameController
    {
        Game game = new Game();
        public Game Game { get; set; }
        GameConsole console = new GameConsole();
        public Int32 _numberOfBots;
        public Int32 _playerBet;

        Boolean moneySpend = false;
        public void GameInitialize()
        {

            game.Player.Name = ConsolePlayerNickname();
            game.Player.Balance = ConsolePlayerBalance();
            ConsoleBotsChoise();
            game.AddBots(_numberOfBots);
        }

        public void ConsoleChoise()
        {
            if (game.Player.Balance <= 0)
            {
                moneySpend = true;
            }
            if (!moneySpend)
            {
                ConsolePlayerBet();
                game.PlayerDealBet(_playerBet);
                game.Deal();
                console.PlayerInfo(game);
            }
            while (!game._goldBlackJack && !game._blackJack && !moneySpend)
            {
                console.PlayerMakeChoise(game);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    game.Stand();
                    console.Score(game);
                    Console.WriteLine("Balance: {0}", game.Player.Balance);
                    console.DealerTakeCard(game);
                    break;
                }
                if (key.Key == ConsoleKey.Spacebar)
                {
                    game.Hit();
                    console.Score(game);
                    console.PlayerTakeCard(game);
                    if (Game.TotalValue(game.Player.Hand) > 21)
                    {
                        GameConsole.PlayerLose();
                        Console.WriteLine("Balance: {0}", game.Player.Balance);
                        break;
                    }
                }
            }
            GameConsole.ContinueOrStopGame();
            if (moneySpend)
            {
                console.BustGame(game);
            }
            ConsoleKeyInfo keyNewGame = Console.ReadKey(true);
            if (keyNewGame.Key == ConsoleKey.N)
            {
                game._blackJack = false;
                game._goldBlackJack = false;
                ConsoleChoise();
            }
            if (keyNewGame.Key == ConsoleKey.Escape)
            {
                console.EndGame(game);
            }
        }
        public void ConsoleBotsChoise()
        {
            Int32 number;
            GameConsole.ConsolePlayerEnterNumberOfBots();
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out number);
            while (number < 0 || number > 5)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out number);
            }
            _numberOfBots = number;
        }

        public String ConsolePlayerNickname()
        {
            GameConsole.ConsolePlayerEnterNickname();
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            while (string.IsNullOrEmpty(inputLine) || inputLine.Contains(" "))
            {
                GameConsole.ConsolePlayerEnterNickname();
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public Decimal ConsolePlayerBalance()
        {
            Int32 balance;
            GameConsole.PlayerEnterBalance();
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out balance);
            while (balance <= 0 || balance >= 1000)
            {
                GameConsole.PlayerEnterBalance();
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out balance);
            }
            return balance;
        }

        public void ConsolePlayerBet()
        {
            console.ConsolePlayerEnterBet(game);
            Int32 bet;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out bet);
            while (bet <= 0 || bet > game.Player.Balance)
            {
                console.ConsolePlayerEnterBet(game);
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
                Int32.TryParse(input, out bet);
            }
            _playerBet = bet;
        }

        public class GameConsole
        {


            public static void ConsolePlayerEnterNickname()
            {
                Console.WriteLine("Enter your nickname: ");
            }
            public static void PlayerEnterBalance()
            {
                Console.WriteLine("Enter your balance: ");
            }
            public static void ConsolePlayerEnterNumberOfBots()
            {
                Console.WriteLine(@"How much bots do you want {0 - 5}?");
            }
            public void ConsolePlayerEnterBet(Game game)
            {
                Console.WriteLine("{1} balance: {0}$.\n", game.Player.Balance, game.Player.Name);
                Console.WriteLine(@"How much bet do you want (0 - {0})?", game.Player.Balance);
            }

            public void PlayerInfo(Game game)
            {
                Console.WriteLine("You score: {2}. You cards: {0}, {1}.\n",
                    game.Player.Hand.ElementAt(0).CardFace + " " + game.Player.Hand.ElementAt(0).CardSuit,
                    game.Player.Hand.ElementAt(1).CardFace + " " + game.Player.Hand.ElementAt(1).CardSuit, Game.TotalValue(game.Player.Hand));
                Console.WriteLine("Dealer score: {0}. Dealer cards: {1}, {2}.\n",
                   Game.TotalValue(game.Dealer.Hand), game.Dealer.Hand.ElementAt(0).CardFace + " " + game.Dealer.Hand.ElementAt(0).CardSuit,
                    game.Dealer.Hand.ElementAt(1).CardFace + " " + game.Dealer.Hand.ElementAt(1).CardSuit);
            }

            public void Score(Game game)
            {
                Console.WriteLine("You score: {0}.\n", Game.TotalValue(game.Player.Hand));
                Console.WriteLine("Dealer score: {0}.\n", Game.TotalValue(game.Dealer.Hand));
            }

            public void PlayerTakeCard(Game game)
            {
                Console.WriteLine("You take {0}.\n", game.Player.Hand.Last().CardFace + " " + game.Player.Hand.Last().CardSuit);
            }

            public void DealerTakeCard(Game game)
            {
                Console.WriteLine("Dealer take {0}.\n", game.Dealer.Hand.Last().CardFace + " " + game.Dealer.Hand.Last().CardSuit);
            }

            public void PlayerMakeChoise(Game game)
            {
                Console.WriteLine("Do you want take card {0}? Press SPACE if want, or ENTER if want continue\n", game.Player.Name);
            }

            public static void PlayerLose()
            {
                Console.WriteLine("You lose!\n");
            }

            public static void PlayerWon()
            {
                Console.WriteLine("You won!\n");
            }

            public static void PlayerDraw()
            {
                Console.WriteLine("Diller and you played in a draw!\n");
            }

            public static void PlayerWonBlackJack()
            {
                Console.WriteLine("You won and have Black Jack!\n");
            }

            public static void PlayerLoseBlackJack()
            {
                Console.WriteLine("You lose! Dealer has Black Jack!\n");
            }

            public static void PlayerDrawBlackJack()
            {
                Console.WriteLine("Diller and you have Black Jack, it is draw!\n");
            }
            public static void ContinueOrStopGame()
            {
                Console.WriteLine("If you want continue game, press N. Else press Escape.\n");
            }
            public void EndGame(Game game)
            {
                Console.WriteLine("Game was stoped. {0} balance: {1}.\n", game.Player.Name, game.Player.Balance);
            }
            public void BustGame(Game game)
            {
                Console.WriteLine("Game was stoped. You dont have money. {0} balance: {1}.\n ..............Press Escape..............\n", game.Player.Name, game.Player.Balance);
            }
            public static void BotsInfo(Bot bot)
            {

                Console.WriteLine("Bot{0} cards 1: {1}, value = {3},\nBot{0} cards 2: {2}, value = {4}.\nBot{0} score = {5}",
                    bot.Id + 1, bot.Hand.ElementAt(0).CardSuit + " " + bot.Hand.ElementAt(0).CardFace,
                      bot.Hand.ElementAt(1).CardSuit + " " + bot.Hand.ElementAt(1).CardFace, bot.Hand.ElementAt(0).CardValue,
                      bot.Hand.ElementAt(1).CardValue, Game.TotalValue(bot.Hand));
            }
            public static void BotTakeCard(Bot bot)
            {
                Console.WriteLine("Bot{0} take card {1}.\n", bot.Id + 1, bot.Hand.Last().CardFace + " " + bot.Hand.Last().CardSuit);
            }
            public static void BotLose(Bot bot)
            {
                Console.WriteLine("Bot{0} lose!\n", bot.Id + 1);
            }

            public static void BotWon(Bot bot)
            {
                Console.WriteLine("Bot{0} won!\n", bot.Id + 1);
            }

            public static void BotDraw(Bot bot)
            {
                Console.WriteLine("Diller and Bot{0} played in a draw!\n", bot.Id + 1);
            }

            public static void BotWonBlackJack(Bot bot)
            {
                Console.WriteLine("Bot{0} won because he has Black Jack!\n", bot.Id + 1);
            }

            public static void BotLoseBlackJack(Bot bot)
            {
                Console.WriteLine("Bot{0} lose because Dealer has Black Jack!\n", bot.Id + 1);
            }

            public static void BotDrawBlackJack(Bot bot)
            {
                Console.WriteLine("Diller and Bot{0} have Black Jack, it is draw!\n", bot.Id + 1);

            }
            public static void BotBalance(Bot bot)
            {
                Console.WriteLine("Bot{0} balance: {1}$\n", bot.Id + 1, bot.Balance);
            }

        }
    }
}
