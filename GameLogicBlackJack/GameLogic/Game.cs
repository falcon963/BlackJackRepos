using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.View;

namespace GameLogicBlackJack.GameLogic
{


    public class Game
    {
        readonly Int32 gameId;
        List<Bot> bots = new List<Bot>();
        private Deck deck = new Deck();
        private GameAction allowedActions;
        private GameState lastState;
        public Player Player { get; set; }
        public Dealer Dealer { get; set; }
        public Bot Bot { get; set; }
        public Boolean blackJack = false;
       // GameController controller;

        public Int32 ReadGameId
        {
            get
            {
                return gameId;
            }
        }

        public Game(Int32 gameId)
        {
            this.gameId = gameId;
            Dealer = new Dealer(1);
            Player = new Player(1);
            LastState = GameState.Unknown;
            AllowedActions = GameAction.None;
            
        }

        public GameAction AllowedActions
        {
            get
            {
                return allowedActions;
            }

            private set
            {
                if (allowedActions != value)
                {
                    allowedActions = value;
                }
            }
        }

        public GameState LastState
        {
            get
            {
                return lastState;
            }
            private set
            {
                if (lastState != value)
                {
                    lastState = value;
                }
            }
        }

        public void AddBots(Int32 countOfBots)
        {
            Int32 number = countOfBots;
            for(Int32 i = 0; i < number ; i++)
            {
                bots.Add(new Bot(i));
            }
        }

        public void PlayerDealBet(Decimal playerBet)
        {
            Player.PlayerBet = playerBet;
            AllowedActions = GameAction.Deal;
        }

        public void Deal()
        {
            if ((AllowedActions & GameAction.Deal) != GameAction.Deal)
            {
                throw new InvalidOperationException();
            }
            LastState = GameState.Unknown;
            if (deck == null)
            {
                deck = new Deck();
            }
            if (deck != null)
            {
                deck.Populate();
                deck.Shuffle();
            }

            foreach(Bot bot in bots)
            {
                bot.botHand.Clear();
                bot.botHand.Add(deck.DrowACard());
                bot.botHand.Add(deck.DrowACard());
               // GameConsole.BotsInfo();
            }
            Dealer.dealerHand.Clear();
            Dealer.dealerHand.Add(deck.DrowACard());
            Dealer.dealerHand.Add(deck.DrowACard());
            Player.playerHand.Clear();
            Player.playerHand.Add(deck.DrowACard());
            Player.playerHand.Add(deck.DrowACard());

            foreach(Bot bot in bots)
            {
                if (bot.TotalValue == 21 & Dealer.TotalValue == 21)
                {

                }
                if (bot.TotalValue == 21 & Dealer.TotalValue != 21)
                {
                    bot.BotBalance += bot.BotBet;
                }
                if (bot.TotalValue != 21 & Dealer.TotalValue == 21)
                {
                    bot.BotBalance -= bot.BotBet;
                }

            }

            if (Player.TotalValue == 21 & Dealer.TotalValue == 21)
            {
                blackJack = true;
                GameController.GameConsole.PlayerDrawBlackJack();
                LastState = GameState.Draw;
                AllowedActions = GameAction.Deal;
            }
            if (Player.TotalValue == 21 & Dealer.TotalValue != 21)
            {
                Player.PlayerBalance += Player.PlayerBet;
                blackJack = true;
                GameController.GameConsole.PlayerWonBlackJack();
                LastState = GameState.PlayerWon;
                AllowedActions = GameAction.Deal;
            }
            if (Player.TotalValue != 21 & Dealer.TotalValue == 21)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                GameController.GameConsole.PlayerLoseBlackJack();
                blackJack = true;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
            }
            if (Player.TotalValue != 21 & Dealer.TotalValue != 21)
            {
                AllowedActions = GameAction.Hit | GameAction.Stand;
            }
        }


        public void Hit()
        {
            if ((AllowedActions & GameAction.Hit) != GameAction.Hit)
            {
                throw new InvalidOperationException();
            }
            Player.playerHand.Add(deck.DrowACard());

            if (Player.TotalValue > 21)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
            }
        }

        public void Stand()
        {
            foreach(Bot bot in bots)
            {

                while (bot.TotalValue < 18)
                {
                    bot.botHand.Add(deck.DrowACard());
                }

                if (Dealer.TotalValue > 21 || bot.TotalValue > Dealer.TotalValue)
                {
                    bot.BotBalance += bot.BotBet;
                }
                if (bot.TotalValue == Dealer.TotalValue)
                {
           
                }
                if (bot.TotalValue < Dealer.TotalValue)
                {
                    bot.BotBalance -= bot.BotBet;
                }
            }

            if ((AllowedActions & GameAction.Stand) != GameAction.Stand)
            {
                throw new InvalidOperationException();
            }

            while (Dealer.TotalValue < 17)
            {
                Dealer.dealerHand.Add(deck.DrowACard());
            }

            if (Dealer.TotalValue > 21 || Player.TotalValue > Dealer.TotalValue)
            {
                Player.PlayerBalance += Player.PlayerBet;
                GameController.GameConsole.PlayerWon();
                LastState = GameState.PlayerWon;
            }
            if(Dealer.TotalValue == Player.TotalValue)
            {
                GameController.GameConsole.PlayerDraw();
                LastState = GameState.Draw;
            }
            if(Dealer.TotalValue <= 21 && Dealer.TotalValue > Player.TotalValue)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                LastState = GameState.DealerWon;
                GameController.GameConsole.PlayerLose();
            }
            AllowedActions = GameAction.Deal;
        }
       
    }
}
