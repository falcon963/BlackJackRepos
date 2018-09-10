using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;

namespace GameLogicBlackJack.GameLogic
{


    public class Game
    {
        readonly Int32 gameId;
        List<Bot> bots = new List<Bot>();
        public Int32 ReadGameId
        {
            get
            {
                return gameId;
            }
        }

        private Deck deck;
        private GameAction allowedActions;
        private GameState lastState;

        public Game(Int32 gameId)
        {
            this.gameId = gameId;
            Bot = new Bot(1);
            Dealer = new Dealer(1);
            Player = new Player(1);
            LastState = GameState.Unknown;
            AllowedActions = GameAction.None;
            
        }

        public event EventHandler LastStateChanged;
        public event EventHandler AllowedActionsChanged;

        public Player Player { get; private set; }
        public Dealer Dealer { get; private set; }
        public Bot Bot { get; private set; }
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

        public void AddBots(Int32 num)
        {
            for(int i = 0; i < num; i++)
            {
                bots.Add(Bot);
            }
        }

        public void Play(Decimal balance, Decimal bet)
        {
            Player.PlayerBalance = balance;
            Player.PlayerBet = bet;
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
            }

            Dealer.hand.Clear();
            Player.hand.Clear();
            for (Int32 i = 0; i > bots.Count; i++)
            {
                Bot.hand.Clear();
            }
            deck.DrowACard(Dealer.hand);
            deck.DrowACard(Player.hand);

            if (Player.hand.TotalValue == 21 & Dealer.hand.TotalValue == 21)
            {
                LastState = GameState.Draw;
                AllowedActions = GameAction.Deal;
            }
            if (Player.hand.TotalValue == 21 & Dealer.hand.TotalValue != 21)
            {
                Player.PlayerBalance += Player.PlayerBet;
                LastState = GameState.PlayerWon;
                AllowedActions = GameAction.Deal;
            }
            if (Player.hand.TotalValue != 21 & Dealer.hand.TotalValue == 21)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
            }
            if (Player.hand.TotalValue != 21 & Dealer.hand.TotalValue != 21)
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
            deck.GiveAdditionalCard(Player.hand);

            if (Player.hand.TotalValue > 21)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
            }
        }

        public void Stand()
        {
            if ((AllowedActions & GameAction.Stand) != GameAction.Stand)
            {
                throw new InvalidOperationException();
            }

            while (Dealer.hand.TotalValue < 17)
            {
                deck.GiveAdditionalCard(Dealer.hand);
            }

            if (Dealer.hand.TotalValue > 21 || Player.hand.TotalValue > Dealer.hand.TotalValue)
            {
                Player.PlayerBalance += Player.PlayerBet;
                LastState = GameState.PlayerWon;
            }
            if(Dealer.hand.TotalValue == Player.hand.TotalValue)
            {
                LastState = GameState.Draw;
            }
            if(Dealer.hand.TotalValue > Player.hand.TotalValue)
            {
                Player.PlayerBalance -= Player.PlayerBet;
                LastState = GameState.DealerWon;
            }
            AllowedActions = GameAction.Deal;
        }
       
    }
}
