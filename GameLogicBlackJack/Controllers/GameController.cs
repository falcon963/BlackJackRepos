using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;
using GameLogicBlackJack.GameLogic;
using GameLogicBlackJack.View;

namespace GameLogicBlackJack.Controllers
{
    public class GameController
    {
        Game game = new Game(2);
        public Game Game { get; set; }
        GameConsole console = new GameConsole();
        public Int32 _numberOfBots;
        public Int32 _playerBet;

        Boolean moneySpend = false;
        public void GameInitialize()
        {

            game.Player.PlayerName = ConsolePlayerNickname();
            game.Player.PlayerBalance = ConsolePlayerBalance();
            ConsoleBotsChoise();
            game.AddBots(_numberOfBots);
        }

        public void ConsoleChoise()
        {
            if (game.Player.PlayerBalance <= 0)
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
            while (!game.blackJack && !moneySpend)
            {
                console.PlayerMakeChoise(game);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    game.Stand();
                    console.Score(game);
                    Console.WriteLine("Balance: {0}", game.Player.PlayerBalance);
                    console.DealerTakeCard(game);
                    break;
                }
                if (key.Key == ConsoleKey.Spacebar)
                {
                    game.Hit();
                    console.Score(game);
                    console.PlayerTakeCard(game);
                    if (game.Player.TotalValue > 21)
                    {
                        GameConsole.PlayerLose();
                        Console.WriteLine("Balance: {0}", game.Player.PlayerBalance);
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
                game.blackJack = false;
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
            while( balance <= 0 || balance >= 1000)
            {
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
            while(bet <= 0 || bet > game.Player.PlayerBalance)
            {
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
                Console.WriteLine("{1} balance: {0}$.", game.Player.PlayerBalance, game.Player.PlayerName);
                Console.WriteLine(@"How much bet do you want (0 - {0})?", game.Player.PlayerBalance);
            }

            public void PlayerInfo(Game game)
            {
                Console.WriteLine("You score: {2}. You cards: {0}, {1}.",
                    game.Player.playerHand.ElementAt(0).CardFace + " " + game.Player.playerHand.ElementAt(0).CardSuit,
                    game.Player.playerHand.ElementAt(1).CardFace + " " + game.Player.playerHand.ElementAt(1).CardSuit, game.Player.TotalValue);
                Console.WriteLine("Dealer score: {0}. Dealer cards: {1}, {2}",
                    game.Dealer.TotalValue, game.Dealer.dealerHand.ElementAt(0).CardFace + " " + game.Dealer.dealerHand.ElementAt(0).CardSuit,
                    game.Dealer.dealerHand.ElementAt(1).CardFace + " " + game.Dealer.dealerHand.ElementAt(1).CardSuit);
            }

            public void Score(Game game)
            {
                Console.WriteLine("You score: {0}.", game.Player.TotalValue);
                Console.WriteLine("Dealer score: {0}.", game.Dealer.TotalValue);
            }

            public void PlayerTakeCard(Game game)
            {
                Console.WriteLine("You take {0}.", game.Player.playerHand.Last().CardFace + " " + game.Player.playerHand.Last().CardSuit);
            }

            public void DealerTakeCard(Game game)
            {
                Console.WriteLine("Dealer take {0}.", game.Dealer.dealerHand.Last().CardFace + " " + game.Dealer.dealerHand.Last().CardSuit);
            }

            public void PlayerMakeChoise(Game game)
            {
                Console.WriteLine("Do you want take card {0}? Press SPACE if want, or ENTER if want continue", game.Player.PlayerName);
            }

            public static void PlayerLose()
            {
                Console.WriteLine("You lose!");
            }

            public static void PlayerWon()
            {
                Console.WriteLine("You won!");
            }

            public static void PlayerDraw()
            {
                Console.WriteLine("Diller and you played in a draw!");
            }

            public static void PlayerWonBlackJack()
            {
                Console.WriteLine("You won and have Black Jack!");
            }

            public static void PlayerLoseBlackJack()
            {
                Console.WriteLine("You lose! Dealer has Black Jack!");
            }

            public static void PlayerDrawBlackJack()
            {
                Console.WriteLine("Diller and you have Black Jack, it is draw!");
            }
            public static void ContinueOrStopGame()
            {
                Console.WriteLine("If you want continue game, press N. Else press Escape.");
            }
            public void EndGame(Game game)
            {
                Console.WriteLine("Game was stoped. {0} balance: {1}.", game.Player.PlayerName, game.Player.PlayerBalance);
            }
            public void BustGame(Game game)
            {
                Console.WriteLine("Game was stoped. You dont have money. {0} balance: {1}.\n ..............Press Escape..............", game.Player.PlayerName, game.Player.PlayerBalance);
            }
            /*  public static void BotsInfo()
              {
                  Console.WriteLine("Bot{0} cards: {1}, {2}", Game.Bot.BotId, Game.Bot.botHand.ElementAt(0).CardSuit + " " + Game.Bot.botHand.ElementAt(0).CardFace,
                      Game.Bot.botHand.ElementAt(1).CardSuit + " " + Game.Bot.botHand.ElementAt(1).CardFace);
              }*/
        }

}
}
