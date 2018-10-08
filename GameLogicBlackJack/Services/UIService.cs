using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.BusinessLogic.Models;

namespace GameLogicBlackJack.Services
{
    public class UIService
    {
        public HomeController controller = new HomeController();

        private static UIService _instance;

        private UIService()
        {
        }

        public static UIService GetInstance()
        {
            if(_instance == null)
            {
                _instance = new UIService();
            }
            return _instance;
        }

        public void DrawPlayerCard()
        {
            controller.TakePlayerCardDeck();
            Console.ForegroundColor = ConsoleColor.Green;
            Int32 countCard = controller.gameView.Player.hand.Count();
            Console.WriteLine("-------------------------------------------------------------------\n");
            for (Int32 i = 0; i < countCard; i++)
            {
                Console.WriteLine("{0} card" + (i + 1) + ": {1}\n", controller.gameView.Player.Name, controller.gameView.Player.hand[i].CardFace + " " + controller.gameView.Player.hand[i].CardSuit);
            }
            Console.WriteLine("{0} score: {1}\n", controller.gameView.Player.Name, controller.service.TotalValue(controller.gameView.Player.hand));
            Console.WriteLine("-------------------------------------------------------------------\n");
        }

        public void DrawBotsCard()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Bot bot in controller.service.game.bots)
            {
                Int32 countCard = bot.hand.Count();
                for (Int32 i = 0; i < countCard; i++)
                {
                    Console.WriteLine("{0} card" + (i + 1) + ": {1}\n", bot.Name, bot.hand[i].CardFace + " " + bot.hand[i].CardSuit);
                }
                Console.WriteLine("{0} score: {1}\n", bot.Name, controller.service.TotalValue(bot.hand));
            }
        }

        public void DrawDealerCard()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Int32 countCard = controller.service.game.Dealer.hand.Count();
            for (Int32 i = 0; i < countCard; i++)
            {
                Console.WriteLine("Dealer card" + (i + 1) + ": {0}\n", controller.service.game.Dealer.hand[i].CardFace + " " + controller.service.game.Dealer.hand[i].CardSuit);
            }
            Console.WriteLine("Dealer score: {0}\n", controller.service.TotalValue(controller.service.game.Dealer.hand));
        }

        public void OutPlayerStatus()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (controller.service.CheckPlayerWon())
            {
                Console.WriteLine("{0}, won!", controller.gameView.Player.Name);
            }
            if (controller.service.CheckPlayerLose())
            {
                Console.WriteLine("{0}, lose!", controller.gameView.Player.Name);
            }
            if (controller.service.CheckPlayerDraw())
            {
                Console.WriteLine("{0}, play a draw!", controller.gameView.Player.Name);
            }
        }

        public void CheckBotStatus()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Bot bot in controller.service.game.bots)
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

        public void AllPlayersList()
        {
            var players = controller.service.GetListPlayers();
            foreach (String player in players)
            {
                Console.WriteLine(player);
            }
        }

        public void PasswordWrongEnterMessege() => Console.WriteLine("Wrong login or password!");
        public void ExitComplitMessege() => Console.WriteLine("Exit complite.");
        public void MenuListMessege() => Console.WriteLine("Menu:\n1|\tLoad\n2|\tPlayers list\n3|\tDelete account\n4|\tNew account\nEsc|\tExit");
        public void PressKeyMessege() => Console.WriteLine("Press Enter if you want continue, or press Space if you want take card\n");
        public void PlayerInfoMessege() => Console.WriteLine("{0} | {1}", controller.service.game.Player.Name, controller.service.game.Player.Balance);
        public void BalancePlayerErrorMessege() => Console.WriteLine("Insufficient funds in the account!");
        public void StartNewGameMessege() => Console.WriteLine("Start new game - press N, stop game - press Esc\n");
        public void GameStopMessege() => Console.WriteLine("Game was stop");
        public void EnterBalanceMessege() => Console.WriteLine("Enter balance:");
        public void EnterPasswordMessege() => Console.WriteLine("Enter you password:");
        public void EnterNicknameMessege() => Console.WriteLine("Enter you nickname:");
        public void DealBetMessege() => Console.WriteLine("Deal bet (1 - {0})", controller.service.game.Player.Balance);
        public void EnterNumBots() => Console.WriteLine("Enter num bots(0 - 5):");
    }
}
