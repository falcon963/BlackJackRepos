using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.BusinessLogic.Models;
using GameLogicBlackJack.Constant;

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

        public void DealBetMessege() => Console.WriteLine("Deal bet (1 - {0})", controller.service.game.Player.Balance);
        public void PlayerInfoMessege() => Console.WriteLine("{0} | {1}", controller.service.game.Player.Name, controller.service.game.Player.Balance);


        public void WrongPasswordEnter() => Console.WriteLine(GameMessenges.WrongPasswordEnter);
        public void ExitComplitMessenge() => Console.WriteLine(GameMessenges.ExitComplitMessenge);
        public void MenuListMessenge() => Console.WriteLine(GameMessenges.MenuListMessenge);
        public void PressKeyMessenge() => Console.WriteLine(GameMessenges.PressKeyMessenge);
        public void BalancePlayerErrorMessenge() => Console.WriteLine(GameMessenges.BalancePlayerErrorMessenge);
        public void StartNewGameMessenge() => Console.WriteLine(GameMessenges.StartNewGameMessenge);
        public void GameStopMessenge() => Console.WriteLine(GameMessenges.GameStopMessenge);
        public void EnterBalanceMessenge() => Console.WriteLine(GameMessenges.EnterBalanceMessenge);
        public void EnterPasswordMessenge() => Console.WriteLine(GameMessenges.EnterPasswordMessenge);
        public void EnterNicknameMessenge() => Console.WriteLine(GameMessenges.EnterNicknameMessenge);
        public void EnterNumBots() => Console.WriteLine(GameMessenges.EnterNumBots);
        public void Line() => Console.WriteLine(GameMessenges.Line);
        public void WrongBalanceEnter() => Console.WriteLine(GameMessenges.WrongBalanceEnter);
        public void WrongNumberOfBots() => Console.WriteLine(GameMessenges.WrongNumberOfBots);
        public void WrongNicknameEnter() => Console.WriteLine(GameMessenges.WrongNicknameEnter);
        public void WrongBetEnter() => Console.WriteLine(GameMessenges.WrongBetEnter);
        public void AccountAccessError() => Console.WriteLine(GameMessenges.AccountAccessError);
        public void DeleteMessenge() => Console.WriteLine(GameMessenges.DeleteMessenge);
        public void ComleteMessenge() => Console.WriteLine(GameMessenges.ComleteMessenge);
    }
}
