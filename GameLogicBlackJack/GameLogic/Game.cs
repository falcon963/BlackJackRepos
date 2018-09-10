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
        public static Player Player { get; set; }
        public static Dealer Dealer { get; set; }
        public static Bot Bot { get; set; }


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

        public void AddBots()
        {
            GameController.ConsoleBotsChoise();
            for(int i = 0; i < GameController.ConsoleBotsChoise(); i++)
            {
                bots.Add(new Bot(i));
                Console.WriteLine("Bot number " + i);
            }
        }

        public void Play()
        {
            GameController.ConsolePlayerNickname();
            Player.PlayerName = GameController.ConsolePlayerNickname();
            GameController.ConsolePlayerBalanse();
            Player.PlayerBalance = GameController.ConsolePlayerBalanse();
            GameController.ConsolePlayerBet();
            Player.PlayerBet = GameController.ConsolePlayerBet();
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
                LastState = GameState.Draw;
                AllowedActions = GameAction.Deal;
            }
            if (Player.TotalValue == 21 & Dealer.TotalValue != 21)
            {
                Player.PlayerBalance += Player.PlayerBet;
                LastState = GameState.PlayerWon;
                AllowedActions = GameAction.Deal;
            }
            if (Player.TotalValue != 21 & Dealer.TotalValue == 21)
            {
                Player.PlayerBalance -= Player.PlayerBet;
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
                LastState = GameState.PlayerWon;
            }
            if(Dealer.TotalValue == Player.TotalValue)
            {
                LastState = GameState.Draw;
            }
            if(Dealer.TotalValue > Player.TotalValue)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                LastState = GameState.DealerWon;
            }
            AllowedActions = GameAction.Deal;
        }
       
    }
}
