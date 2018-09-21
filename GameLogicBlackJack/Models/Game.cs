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

namespace GameLogicBlackJack.Models
{


    public class Game
    {
        List<Bot> bots = new List<Bot>();
        private Deck deck = new Deck();
        private GameAction allowedActions;
        private GameState lastState;
        public Int32 PlayerId { get; set; }
        public Int32 DealerId { get; set; }
        public Int32 BotId { get; set; }
        public Boolean _blackJack = false;
        public Boolean _goldBlackJack = false;
        public Boolean[] _botBlackJack = { false, false, false, false, false};
        public Boolean[] _botGoldBlackJack = { false, false, false, false, false };
        public Int32 GameId { get; set; }



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


        public Game(Int32 gameId)
        {
            this.gameId = gameId;
            Dealer = new Dealer();
            Player = new Player();
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
                bots.Add(new Bot());
            }
        }

        public void PlayerDealBet(Decimal playerBet)
        {
            Player.Bet = playerBet;
            AllowedActions = GameAction.Deal;
        }

        public void GoldBlackJackPlayerCheckup(List<Card> dealer, List<Card> player)
        {
            if((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (player[0].CardFace == CardFaceEnum.Ace && player[1].CardFace == CardFaceEnum.Ace))
            {
                GameController.GameConsole.PlayerDraw();
                _goldBlackJack = true;
            }
            if((player[0].CardFace == CardFaceEnum.Ace && player[1].CardFace == CardFaceEnum.Ace) && !_goldBlackJack)
            {
                Player.Balance += Player.Bet;
                GameController.GameConsole.PlayerWon();
                _goldBlackJack = true;
            }
            if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && !_goldBlackJack)
            {
                Player.Balance -= Player.Bet;
                GameController.GameConsole.PlayerLose();
                _goldBlackJack = true;
            }
        }

        public void GoldBlackJackBotCheckup(List<Card> dealer, List<Bot> bots)
        {
            foreach (Bot bot in bots)
            {
                if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (bot.Hand[0].CardFace == CardFaceEnum.Ace && bot.Hand[1].CardFace == CardFaceEnum.Ace))
                {
                    GameController.GameConsole.BotWon(bot);
                    _botGoldBlackJack[Bot.Id] = true;
                }
                if ((bot.Hand[0].CardFace == CardFaceEnum.Ace && bot.Hand[1].CardFace == CardFaceEnum.Ace) && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.PlayerWon();
                    _botGoldBlackJack[Bot.Id] = true;
                }
                if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.PlayerLose();
                    _botGoldBlackJack[Bot.Id] = true;
                }
            }
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

     

            

            GoldBlackJackBotCheckup(Dealer.Hand, bots);
            GoldBlackJackPlayerCheckup(Dealer.Hand, Player.Hand);

            foreach (Bot bot in bots)
            {
                if (TotalValue(bot.Hand) == 21 & TotalValue(Dealer.Hand) == 21 & !_botGoldBlackJack[bot.Id])
                {
                    GameController.GameConsole.BotDrawBlackJack(bot);
                    _botBlackJack[bot.Id] = true;
                }
                if (TotalValue(bot.Hand) == 21 & TotalValue(Dealer.Hand) !=21 & !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWonBlackJack(bot);
                    _botBlackJack[bot.Id] = true;
                }
                if (TotalValue(bot.Hand) != 21 & TotalValue(Dealer.Hand) == 21 & !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLoseBlackJack(bot);
                    _botBlackJack[bot.Id] = true;
                }

            }

            if (TotalValue(Player.Hand) == 21 & TotalValue(Dealer.Hand) == 21 & !_goldBlackJack)
            {
                _blackJack = true;
                GameController.GameConsole.PlayerDrawBlackJack();
                LastState = GameState.Draw;
                AllowedActions = GameAction.Deal;
            }
            if (TotalValue(Player.Hand) == 21 & TotalValue(Dealer.Hand) != 21 & !_goldBlackJack)
            {
                Player.Balance += Player.Bet;
                _blackJack = true;
                GameController.GameConsole.PlayerWonBlackJack();
                LastState = GameState.PlayerWon;
                AllowedActions = GameAction.Deal;
                BotAndDealerPlay();
            }
            if (TotalValue(Player.Hand) != 21 & TotalValue(Dealer.Hand) == 21 & !_goldBlackJack)
            {
                Player.Balance -= Player.Bet;
                GameController.GameConsole.PlayerLoseBlackJack();
                _blackJack = true;
                LastState = GameState.DealerWon;
                AllowedActions = GameAction.Deal;
            }
            if (TotalValue(Player.Hand) != 21 & TotalValue(Dealer.Hand) != 21 & !_goldBlackJack)
            {
                AllowedActions = GameAction.Hit | GameAction.Stand;
            }
        }


        public void BotPlaying()
        {
            foreach (Bot bot in bots)
            {

                while (TotalValue(bot.Hand) < 18 && !_botGoldBlackJack[bot.Id])
                {
                    bot.Hand.Add(deck.DrowACard());
                    GameController.GameConsole.BotTakeCard(bot);
                }
            }
        }

        public void Hit()
        {
            if ((AllowedActions & GameAction.Hit) != GameAction.Hit)
            {
                throw new InvalidOperationException();
            }
            Player.Hand.Add(deck.DrowACard());

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
                Dealer.Hand.Add(deck.DrowACard());
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
            if (TotalValue(Dealer.Hand) > 21 && TotalValue(Player.Hand) > 21)
            {
                GameController.GameConsole.PlayerDraw();
                LastState = GameState.Draw;
            }
            if (TotalValue(Dealer.Hand) <= 21 && TotalValue(Dealer.Hand) > TotalValue(Player.Hand))
            {
                Player.Balance -= Player.Bet;
                LastState = GameState.DealerWon;
                GameController.GameConsole.PlayerLose();
            }
            foreach (Bot bot in bots)
            {
                GameController.GameConsole.BotsInfo(bot);
                if (TotalValue(Dealer.Hand) > 21 && TotalValue(bot.Hand) <= 21 && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if ((TotalValue(bot.Hand) > TotalValue(Dealer.Hand)) && !(_botBlackJack[bot.Id]) && TotalValue(bot.Hand) <= 21 && TotalValue(Dealer.Hand) < 21 && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if (TotalValue(bot.Hand) > 21 && TotalValue(Dealer.Hand) > 21 && !(_botBlackJack[bot.Id]) && !_botGoldBlackJack[bot.Id])
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.Hand) == TotalValue(Dealer.Hand)) && !(_botBlackJack[bot.Id]) && TotalValue(bot.Hand) <= 21 && !_botGoldBlackJack[bot.Id])
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.Hand) < TotalValue(Dealer.Hand)) && (TotalValue(Dealer.Hand) <= 21 && TotalValue(bot.Hand) < 21) && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
                if (TotalValue(bot.Hand) > 21 && TotalValue(Dealer.Hand) <= 21 && !(_botBlackJack[bot.Id]) && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
            }
            AllowedActions = GameAction.Deal;
        }
       
        public void BotAndDealerPlay()
        {
            BotPlaying();
            while (TotalValue(Dealer.Hand) < 17)
            {
                Dealer.Hand.Add(deck.DrowACard());
            }
            foreach (Bot bot in bots)
            {
                GameController.GameConsole.BotsInfo(bot);
                if (TotalValue(Dealer.Hand) > 21 && TotalValue(bot.Hand) <= 21 && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if ((TotalValue(bot.Hand) > TotalValue(Dealer.Hand)) && !(_botBlackJack[bot.Id]) && TotalValue(bot.Hand) <= 21 && TotalValue(Dealer.Hand) < 21 && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance += bot.Bet;
                    GameController.GameConsole.BotWon(bot);
                }
                if (TotalValue(bot.Hand) > 21 && TotalValue(Dealer.Hand) > 21 && !(_botBlackJack[bot.Id]) && !_botGoldBlackJack[bot.Id])
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.Hand) == TotalValue(Dealer.Hand)) && !(_botBlackJack[bot.Id]) && TotalValue(bot.Hand) <= 21 && !_botGoldBlackJack[bot.Id])
                {
                    GameController.GameConsole.BotDraw(bot);
                }
                if ((TotalValue(bot.Hand) < TotalValue(Dealer.Hand)) && (TotalValue(Dealer.Hand) <= 21 && TotalValue(bot.Hand) < 21) && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
                if (TotalValue(bot.Hand) > 21 && TotalValue(Dealer.Hand) <= 21 && !(_botBlackJack[bot.Id]) && !_botGoldBlackJack[bot.Id])
                {
                    bot.Balance -= bot.Bet;
                    GameController.GameConsole.BotLose(bot);
                }
            }
        }
        }
    }

