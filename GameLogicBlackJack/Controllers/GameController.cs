using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Models;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;
using GameLogicBlackJack.Services;

namespace GameLogicBlackJack.Controllers
{
    public class GameController
    {
        public Game game = new Game();
        GameConsole console = new GameConsole();
        public Int32 _numberOfBots;
        public Int32 _playerBet;
        Boolean moneySpend = false;
        public GameService GameService { get; set; }


        public void GameInitialize()
        {

            game.Player.Name = ConsolePlayerNickname();
            game.Player.Balance = ConsolePlayerBalance();
            ConsoleBotsChoise();
            GameService.BotsInitialize(game);
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
                PlayerDealBet(_playerBet);
                Deal();
                console.PlayerInfo(game);
            }
            while (!game.Player.PlayerWon  && !moneySpend)
            {
                console.PlayerMakeChoise(game);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Stand();
                    console.Score(game);
                    Console.WriteLine("Balance: {0}", game.Player.Balance);
                    console.DealerTakeCard(game);
                    break;
                }
                if (key.Key == ConsoleKey.Spacebar)
                {
                    Hit();
                    console.Score(game);
                    console.PlayerTakeCard(game);
                    if (TotalValue(game.Player.hand) > 21)
                    {
                        GameConsole.PlayerLose();
                        Console.WriteLine("Balance: {0}", game.Player.Balance);
                        break;
                    }
                }
            }


            if(game.LastState == GameState.PlayerWon)
            {
                game.Player.PlayerWon = true;
            }


            if(game.LastState == GameState.Draw)
            {
                game.Player.PlayerDraw = true;
            }


            foreach(Bot bot in game.bots)
            {
                if(bot.BotState == BotState.BotWon)
                {
                    bot.BotWon = true;
                }
                if(bot.BotState == BotState.BotDraw)
                {
                    bot.BotDraw = true;
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
                GameService.PlayerSave(game);
                GameService.DealerSave(game);
                GameService.BotsSave(game);
                GameService.GameSave(game);
                ConsoleChoise();
            }
            if (keyNewGame.Key == ConsoleKey.Escape)
            {
                GameService.PlayerSave(game);
                GameService.DealerSave(game);
                GameService.BotsSave(game);
                GameService.GameSave(game);
                console.EndGame(game);
            }
        }

        public static Int32 TotalValue(List<Card> hand)
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


        public void PlayerDealBet(Int32 playerBet)
        {
            game.Player.Bet = playerBet;
            game.AllowedActions = GameAction.Deal;
        }

        public void GoldBlackJackPlayerCheckup(List<Card> dealer, List<Card> player)
        {
            if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (player[0].CardFace == CardFaceEnum.Ace && player[1].CardFace == CardFaceEnum.Ace) && (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                GameController.GameConsole.PlayerDraw();
                game.LastState = GameState.Draw;
            }
            if ((player[0].CardFace == CardFaceEnum.Ace && player[1].CardFace == CardFaceEnum.Ace) && (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance += game.Player.Bet;
                GameController.GameConsole.PlayerWon();
                game.LastState = GameState.PlayerWon;
            }
            if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Player.Bet;
                GameController.GameConsole.PlayerLose();
                game.LastState = GameState.PlayerLose;
            }
        }

        public void GoldBlackJackBotCheckup(List<Card> dealer, List<Bot> bots)
        {
            foreach (Bot bot in bots)
            {
                if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (bot.hand[0].CardFace == CardFaceEnum.Ace && bot.hand[1].CardFace == CardFaceEnum.Ace) && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    GameController.GameConsole.BotDraw(bot);
                    bot.BotState = BotState.BotDraw;
                }
                if ((bot.hand[0].CardFace == CardFaceEnum.Ace && bot.hand[1].CardFace == CardFaceEnum.Ace) && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.PlayerWon();
                    bot.BotState = BotState.BotWon;
                }
                if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.PlayerLose();
                    bot.BotState = BotState.BotLose;
                }
            }
        }

        public void Deal()
        {
            game.LastState = GameState.Unknown;
            if (game.deck == null)
            {
                game.deck = new Deck();
            }
            if (game.deck != null)
            {
                game.deck.Populate();
                game.deck.Shuffle();
            }

            foreach (Bot bot in game.bots)
            {
                //  GameController.GameConsole.BotBalance(bot);
                bot.hand.Clear();
                bot.hand.Add(game.deck.DrowACard());
                bot.hand.Add(game.deck.DrowACard());
                GameController.GameConsole.BotsInfo(bot);
            }
            game.Dealer.Clear();
            game.Dealer.hand.Add(game.deck.DrowACard());
            game.Dealer.hand.Add(game.deck.DrowACard());
            game.Player.Clear();
            game.Player.hand.Add(game.deck.DrowACard());
            game.Player.hand.Add(game.deck.DrowACard());





            GoldBlackJackBotCheckup(game.Dealer.hand, game.bots);
            GoldBlackJackPlayerCheckup(game.Dealer.hand, game.Player.hand);

            foreach (Bot bot in game.bots)
            {
                if (TotalValue(bot.hand) == 21 & TotalValue(game.Dealer.hand) == 21 & (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    GameController.GameConsole.BotDrawBlackJack(bot);
                    bot.BotState = BotState.BotDraw;
                }
                if (TotalValue(bot.hand) == 21 & TotalValue(game.Dealer.hand) != 21 & (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWonBlackJack(bot);
                    bot.BotState = BotState.BotWon;
                }
                if (TotalValue(bot.hand) != 21 & TotalValue(game.Dealer.hand) == 21 & (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLoseBlackJack(bot);
                    bot.BotState = BotState.BotLose;
                }

            }

            if (TotalValue(game.Player.hand) == 21 & TotalValue(game.Dealer.hand) == 21 & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                GameController.GameConsole.PlayerDrawBlackJack();
                game.LastState = GameState.Draw;
                game.AllowedActions = GameAction.Deal;
            }
            if (TotalValue(game.Player.hand) == 21 & TotalValue(game.Dealer.hand) != 21 & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance += game.Player.Bet;
                GameController.GameConsole.PlayerWonBlackJack();
                game.LastState = GameState.PlayerWon;
                game.AllowedActions = GameAction.Deal;
                BotAndDealerPlay();
            }
            if (TotalValue(game.Player.hand) != 21 & TotalValue(game.Dealer.hand) == 21 & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Player.Bet;
                GameController.GameConsole.PlayerLoseBlackJack();
                game.LastState = GameState.PlayerLose;
                game.AllowedActions = GameAction.Deal;
            }
            if (TotalValue(game.Player.hand) != 21 & TotalValue(game.Dealer.hand) != 21 & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.AllowedActions = GameAction.Hit | GameAction.Stand;
            }
        }


        public void BotPlaying()
        {
            foreach (Bot bot in game.bots)
            {

                while (TotalValue(bot.hand) < 18 && !bot.BotWon && !bot.BotDraw)
                {
                    bot.hand.Add(game.deck.DrowACard());
                    GameController.GameConsole.BotTakeCard(bot);
                }
            }
        }

        public void Hit()
        {
            game.Player.hand.Add(game.deck.DrowACard());

            if (TotalValue(game.Player.hand) > 21 & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Player.Bet;
                game.LastState = GameState.PlayerLose;
                BotAndDealerPlay();
            }
        }

        public void Stand()
        {

            BotPlaying();

            while (TotalValue(game.Dealer.hand) < 17)
            {
                game.Dealer.hand.Add(game.deck.DrowACard());
            }



            if (TotalValue(game.Dealer.hand) > 21 || TotalValue(game.Player.hand) > TotalValue(game.Dealer.hand) & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance += game.Player.Bet;
                GameController.GameConsole.PlayerWon();
                game.LastState = GameState.PlayerWon;
            }
            if (TotalValue(game.Dealer.hand) == TotalValue(game.Player.hand) & (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                GameController.GameConsole.PlayerDraw();
                game.LastState = GameState.Draw;
            }
            if (TotalValue(game.Dealer.hand) > 21 && TotalValue(game.Player.hand) > 21 && (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                GameController.GameConsole.PlayerDraw();
                game.LastState = GameState.Draw;
            }
            if (TotalValue(game.Dealer.hand) <= 21 && TotalValue(game.Dealer.hand) > TotalValue(game.Player.hand) && (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Player.Bet;
                game.LastState = GameState.PlayerLose;
                GameController.GameConsole.PlayerLose();
            }
            foreach (Bot bot in game.bots)
            {
                GameController.GameConsole.BotsInfo(bot);
                if (TotalValue(game.Dealer.hand) > 21 && TotalValue(bot.hand) <= 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if ((TotalValue(bot.hand) > TotalValue(game.Dealer.hand)) &&  TotalValue(bot.hand) <= 21 && TotalValue(game.Dealer.hand) < 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) > 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.hand) == TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.hand) < TotalValue(game.Dealer.hand)) && (TotalValue(game.Dealer.hand) <= 21 && TotalValue(bot.hand) < 21) && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) <= 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
            }
            game.AllowedActions = GameAction.Deal;
        }

        public void BotAndDealerPlay()
        {
            BotPlaying();
            while (TotalValue(game.Dealer.hand) < 17)
            {
                game.Dealer.hand.Add(game.deck.DrowACard());
            }
            foreach (Bot bot in game.bots)
            {
                GameController.GameConsole.BotsInfo(bot);
                if (TotalValue(game.Dealer.hand) > 21 && TotalValue(bot.hand) <= 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if ((TotalValue(bot.hand) > TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && TotalValue(game.Dealer.hand) < 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) > 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.hand) == TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.hand) < TotalValue(game.Dealer.hand)) && (TotalValue(game.Dealer.hand) <= 21 && TotalValue(bot.hand) < 21) && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) <= 21 && (bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
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

            public Int32 ConsolePlayerBalance()
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
                    game.Player.hand.ElementAt(0).CardFace + " " + game.Player.hand.ElementAt(0).CardSuit,
                    game.Player.hand.ElementAt(1).CardFace + " " + game.Player.hand.ElementAt(1).CardSuit, TotalValue(game.Player.hand));
                Console.WriteLine("Dealer score: {0}. Dealer cards: {1}, {2}.\n",
                   TotalValue(game.Dealer.hand), game.Dealer.hand.ElementAt(0).CardFace + " " + game.Dealer.hand.ElementAt(0).CardSuit,
                    game.Dealer.hand.ElementAt(1).CardFace + " " + game.Dealer.hand.ElementAt(1).CardSuit);
            }

            public void Score(Game game)
            {
                Console.WriteLine("You score: {0}.\n", TotalValue(game.Player.hand));
                Console.WriteLine("Dealer score: {0}.\n", TotalValue(game.Dealer.hand));
            }

            public void PlayerTakeCard(Game game)
            {
                Console.WriteLine("You take {0}.\n", game.Player.hand.Last().CardFace + " " + game.Player.hand.Last().CardSuit);
            }

            public void DealerTakeCard(Game game)
            {
                Console.WriteLine("Dealer take {0}.\n", game.Dealer.hand.Last().CardFace + " " + game.Dealer.hand.Last().CardSuit);
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
                    bot.Id + 1, bot.hand.ElementAt(0).CardSuit + " " + bot.hand.ElementAt(0).CardFace,
                      bot.hand.ElementAt(1).CardSuit + " " + bot.hand.ElementAt(1).CardFace, bot.hand.ElementAt(0).CardValue,
                      bot.hand.ElementAt(1).CardValue, TotalValue(bot.hand));
            }
            public static void BotTakeCard(Bot bot)
            {
                Console.WriteLine("Bot{0} take card {1}.\n", bot.Id + 1, bot.hand.Last().CardFace + " " + bot.hand.Last().CardSuit);
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
