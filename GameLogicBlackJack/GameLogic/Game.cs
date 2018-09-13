using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.View;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

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
        public Boolean[] botBlackJack = { false, false, false, false, false};
        public Boolean[] botBusting = { false, false, false, false, false };

        public static Int32 TotalValue(List<Card> hand)
        {
                Int32 totalValue = 0;
                foreach (Card card in hand)
                {
                    if ((Int32)card.CardFace > 1 & (Int32)card.CardFace < 11)
                    {
                        card.CardValue = (Int32)card.CardFace;
                    }
                    if ((Int32)card.CardFace >= 11)
                    {
                        card.CardValue = 10;
                    }
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
            Player.Bet = playerBet;
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
              //  GameController.GameConsole.BotBalance(bot);
                bot.Hand.Clear();
                bot.Hand.Add(deck.DrowACard());
                bot.Hand.Add(deck.DrowACard());
                GameController.GameConsole.BotsInfo(bot);
            }
            Dealer.Hand.Clear();
            Dealer.Hand.Add(deck.DrowACard());
            Dealer.Hand.Add(deck.DrowACard());
            Player.Hand.Clear();
            Player.Hand.Add(deck.DrowACard());
            Player.Hand.Add(deck.DrowACard());

     

            foreach(Bot bot in bots)
            {
                if (TotalValue(bot.Hand) == 21 & TotalValue(Dealer.Hand) == 21)
                {
                    GameController.GameConsole.BotDrawBlackJack(bot);
                    botBlackJack[bot.Id] = true;
                }
                if (TotalValue(bot.Hand) == 21 & TotalValue(Dealer.Hand) != 21)
                {
                    bot.botBalance += bot.Bet;
                    GameController.GameConsole.BotWonBlackJack(bot);
                    botBlackJack[bot.Id] = true;
                }
                if (TotalValue(bot.Hand) != 21 & TotalValue(Dealer.Hand) == 21)
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLoseBlackJack(bot);
                    botBlackJack[bot.Id] = true;
                }

            }


            if (TotalValue(Player.Hand) == 21 & TotalValue(Dealer.Hand) == 21)
            {
                blackJack = true;
                GameController.GameConsole.PlayerDrawBlackJack();
                LastState = GameState.Draw;
                AllowedActions = GameAction.Deal;
            }
            if (TotalValue(Player.Hand) == 21 & TotalValue(Dealer.Hand) != 21)
            {
                Player.Balance += Player.Bet;
                blackJack = true;
                GameController.GameConsole.PlayerWonBlackJack();
                LastState = GameState.PlayerWon;
                AllowedActions = GameAction.Deal;
                BotAndDealerPlay();
            }
            if (TotalValue(Player.Hand) != 21 & TotalValue(Dealer.Hand) == 21)
            {
                Player.Balance -= Player.Bet;
                GameController.GameConsole.PlayerLoseBlackJack();
                blackJack = true;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
            }
            if (TotalValue(Player.Hand) != 21 & TotalValue(Dealer.Hand) != 21)
            {
                AllowedActions = GameAction.Hit | GameAction.Stand;
            }
        }


        public void BotPlaying()
        {
            foreach (Bot bot in bots)
            {

                while (TotalValue(bot.Hand) < 18)
                {
                    bot.botHand.Add(deck.DrowACard());
                    GameController.GameConsole.BotTakeCard(bot);
                }

                if(TotalValue(bot.Hand) > 21)
                {
                    botBusting[bot.Id] = true;
                    GameController.GameConsole.BotLose(bot);
                    bot.Balance -= bot.Bet;
                }
            }
        }

        public void Hit()
        {
            if ((AllowedActions & GameAction.Hit) != GameAction.Hit)
            {
                throw new InvalidOperationException();
            }
            Player.playerHand.Add(deck.DrowACard());

            if (TotalValue(Player.Hand) > 21)
            {
                Player.Balance -= Player.Bet;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
                BotAndDealerPlay();
            }
        }

        public void Stand()
        {
           

            if ((AllowedActions & GameAction.Stand) != GameAction.Stand)
            {
                throw new InvalidOperationException();
            }

            BotPlaying();

            while (TotalValue(Dealer.Hand) < 17)
            {
                Dealer.dealerHand.Add(deck.DrowACard());
            }

        

            if (TotalValue(Dealer.Hand) > 21 || TotalValue(Player.Hand) > TotalValue(Dealer.Hand))
            {
                Player.Balance += Player.Bet;
                GameController.GameConsole.PlayerWon();
                LastState = GameState.PlayerWon;
            }
            if(TotalValue(Dealer.Hand) == TotalValue(Player.Hand))
            {
                GameController.GameConsole.PlayerDraw();
                LastState = GameState.Draw;
            }
            if(TotalValue(Dealer.Hand) <= 21 && TotalValue(Dealer.Hand) > TotalValue(Player.Hand))
            {
                Player.Balance -= Player.Bet;
                LastState = GameState.DealerWon;
                GameController.GameConsole.PlayerLose();
            }
            foreach (Bot bot in bots)
            {
                if (TotalValue(Dealer.Hand) > 21 || TotalValue(bot.Hand) > TotalValue(Dealer.Hand) && !(TotalValue(bot.Hand) > 21) && !botBlackJack[bot.Id] && !botBusting[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if (TotalValue(bot.Hand) == TotalValue(Dealer.Hand) && !(TotalValue(bot.Hand) > 21) && !botBlackJack[bot.Id] && !botBusting[bot.Id])
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if (TotalValue(bot.Hand) < TotalValue(Dealer.Hand) && !(TotalValue(Dealer.Hand) > 21) && !botBlackJack[bot.Id] && !botBusting[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
            }
            AllowedActions = GameAction.Deal;

            BotPlaying();
        }
       
        public void BotAndDealerPlay()
        {
            BotPlaying();
            while (TotalValue(Dealer.Hand) < 17)
            {
                Dealer.dealerHand.Add(deck.DrowACard());
            }
            foreach (Bot bot in bots)
            {
                if (TotalValue(Dealer.Hand) > 21 || TotalValue(bot.Hand) > TotalValue(Dealer.Hand) && !(TotalValue(bot.Hand) > 21) && !botBlackJack[bot.Id] && !botBusting[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if (TotalValue(bot.Hand) == TotalValue(Dealer.Hand) && !(TotalValue(bot.Hand) > 21) && !botBlackJack[bot.Id] && !botBusting[bot.Id])
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if (TotalValue(bot.Hand) < TotalValue(Dealer.Hand) || (TotalValue(bot.Hand) > 21) && !(TotalValue(Dealer.Hand) > 21) && !botBlackJack[bot.Id] && !botBusting[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
            }
        }
        }
    }

