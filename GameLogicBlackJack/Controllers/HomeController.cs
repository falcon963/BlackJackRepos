using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.ViewModels;
using GameLogicBlackJack.BusinessLogic.Models;
using GameLogicBlackJack.BusinessLogic.Services;
using GameLogicBlackJack.Services;

namespace GameLogicBlackJack.Controllers
{
    public class HomeController
    {
        public GameService service;
        public GameViewModel gameView = new GameViewModel();
        public HomeController()
        {
            service =  new GameService(); 
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

        public void Launcher()
        {

            while (true)
            {
                UIService.GetInstance().MenuListMessege();
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.NumPad1)//+
                {
                    UIService.GetInstance().EnterNicknameMessege();
                    var login = PlayerEnterData();
                    while (string.IsNullOrEmpty(login) || login.Contains(" "))
                    {
                        //error messege
                        login = PlayerEnterData();
                    }

                    UIService.GetInstance().EnterPasswordMessege();
                    var password = PlayerEnterData();
                    while (string.IsNullOrEmpty(password) || password.Contains(" "))
                    {
                        //error messege
                        password = PlayerEnterData();
                    }

                    var account = service.VerifyHashedPassword(login, password);
                        if (account != null)
                        {
                            gameView.Player.Name = account.Name;
                            gameView.Player.Balance = account.Balance;
                            gameView.Player.Password = account.Password;
                            gameView.Player.Id = account.Id;
                            UpdatePlayerInBL();

                        UIService.GetInstance().EnterNumBots();
                            var botNumber = PlayerEnterData();
                            while (botNumber.Any(c => char.IsLetter(c)) || Convert.ToInt32(botNumber) < 0 || Convert.ToInt32(botNumber) > 5 || string.IsNullOrEmpty(botNumber) || botNumber.Contains(" "))
                            {
                            //error messege
                            botNumber = PlayerEnterData();
                            }
                            service.BotAdd(Convert.ToInt32(botNumber));
                            GameStart();
                        }
                }
                if (key.Key == ConsoleKey.NumPad2)
                {
                    UIService.GetInstance().AllPlayersList();
                }
                if (key.Key == ConsoleKey.NumPad3)//+
                {
                    UIService.GetInstance().EnterNicknameMessege();
                    var login = PlayerEnterData();
                    while (string.IsNullOrEmpty(login) || login.Contains(" "))
                    {
                        //error messege
                        login = PlayerEnterData();
                    }

                    UIService.GetInstance().EnterPasswordMessege();
                    var password = PlayerEnterData();
                    while (string.IsNullOrEmpty(password) || password.Contains(" "))
                    {
                        //error messege
                        password = PlayerEnterData();
                    }

                    service.DeletePlayer(login, password);
                }
                if (key.Key == ConsoleKey.NumPad4)//new account
                {
                    NewGameInitialize();
                    GameStart();
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    UIService.GetInstance().ExitComplitMessege();
                    break;
                }
            }
        }

        public void GameStart()//+
        {
                if (service.CheckBalance() <= 0)
                {
                    UIService.GetInstance().BalancePlayerErrorMessege();

                UIService.GetInstance().EnterBalanceMessege();
                    var balance = PlayerEnterData();
                    while (balance.Any(c => char.IsLetter(c)) || Convert.ToInt32(balance) <= 0 || Convert.ToInt32(balance) > 5000 || string.IsNullOrEmpty(balance) || balance.Contains(" "))
                    {
                    //error messege
                    balance = PlayerEnterData();
                    }
                    gameView.Player.Balance = Convert.ToInt32(balance);

                    UpdatePlayerInBL();
                }

            UIService.GetInstance().DealBetMessege();
                var bet = PlayerEnterData();
                while (bet.Any(c => char.IsLetter(c)) || Convert.ToInt32(bet) <= 0 || Convert.ToInt32(bet) > service.game.Player.Balance || string.IsNullOrEmpty(bet) || bet.Contains(" "))
                {
                //error messege
                bet = PlayerEnterData();
                }
                gameView.Bet = Convert.ToInt32(bet);

                UpdateGameInBL();
                service.GameDeal();
                UIService.GetInstance().DrawPlayerCard();
                UIService.GetInstance().DrawBotsCard();
                UIService.GetInstance().DrawDealerCard();
                UIService.GetInstance().OutPlayerStatus();

                while (!service.CheckPlayerStatus())
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    UIService.GetInstance().PressKeyMessege();
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        service.PlayerStandGame();
                        UIService.GetInstance().DrawDealerCard();
                        break;
                    }
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        service.PlayerHitGame();
                        UIService.GetInstance().DrawPlayerCard();
                    }
                }

                UIService.GetInstance().DrawBotsCard();
                UIService.GetInstance().DrawDealerCard();
                UIService.GetInstance().CheckBotStatus();
                UIService.GetInstance().OutPlayerStatus();
                service.DealerSave();
                service.GameSave();
                service.BotsSave();
                UIService.GetInstance().PlayerInfoMessege();

                Console.ForegroundColor = ConsoleColor.White;
                service.PlayerSave();
                UIService.GetInstance().StartNewGameMessege();
                ConsoleKeyInfo keyNewGame = Console.ReadKey(true);
                if (keyNewGame.Key == ConsoleKey.N)
                {
                    GameStart();
                }
                if (keyNewGame.Key == ConsoleKey.Escape)
                {
                    UIService.GetInstance().GameStopMessege();
                }
            
        }


        

        public void NewGameInitialize()//+
        {
            UIService.GetInstance().EnterNicknameMessege();
            var login = PlayerEnterData();
            while (string.IsNullOrEmpty(login) || login.Contains(" "))
            {
                //error messege
                login = PlayerEnterData();
            }
            gameView.Player.Name = login;

            UIService.GetInstance().EnterPasswordMessege();
            var password = PlayerEnterData();
            while (string.IsNullOrEmpty(password) || password.Contains(" "))
            {
                //error messege
                password = PlayerEnterData();
            }
            gameView.Player.Password = service.HashPassword(password);

            UIService.GetInstance().EnterBalanceMessege();
            var balance = PlayerEnterData();
            while(balance.Any(c => char.IsLetter(c)) || Convert.ToInt32(balance) <= 0 || Convert.ToInt32(balance) > 5000 || string.IsNullOrEmpty(balance) || balance.Contains(" "))
            {
                //error messege
                balance = PlayerEnterData();
            }
            gameView.Player.Balance = Convert.ToInt32(balance);


            UpdatePlayerInBL();
            service.PlayerSave();

            UIService.GetInstance().EnterNumBots();
            var botNumber = PlayerEnterData();
            while (botNumber.Any(c => char.IsLetter(c)) || Convert.ToInt32(botNumber) < 0 || Convert.ToInt32(botNumber) > 5 || string.IsNullOrEmpty(botNumber) || botNumber.Contains(" "))
            {
                //error messege
                botNumber = PlayerEnterData();
            }
            service.BotAdd(Convert.ToInt32(botNumber));
        }


        public void UpdateGameInBL()
        {
            service.game.Bet = gameView.Bet;
        }

        public void UpdatePlayerInBL()
        {
            service.game.Player.Name = gameView.Player.Name;
            service.game.Player.Password = gameView.Player.Password;
            service.game.Player.Balance = gameView.Player.Balance;
        }


        public String PlayerEnterData()
        {
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            return input;
        }
    }
}
