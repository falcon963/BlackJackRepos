using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Models;
using GameLogicBlackJack.DataAccess.Entities;
using GameLogicBlackJack.DataAccess.Interfaces;
using GameLogicBlackJack.DataAccess.SQLite;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enums;
using GameLogicBlackJack.DataAccess.Repositories;
using System.Security.Cryptography;

namespace GameLogicBlackJack.BusinessLogic.Services
{
    public class GameService : IGameService
    {
        public SQLiteUnitOfWork Database { get; private set; }


        public GameService()
        {
           Database = unit;
        }
        SQLiteUnitOfWork unit = new SQLiteUnitOfWork();
        public Game game = new Game();


        public void BotAdd(Int32 num)
        {
            if (num >= 1)
            {
                game.bots.Add(new Bot { Name = "Jim", Balance = 500, Bet = 20 });
            }
            if (num >= 2)
            {
                game.bots.Add(new Bot { Name = "Fill", Balance = 600, Bet = 20 });
            }
            if (num >= 3)
            {
                game.bots.Add(new Bot { Name = "Sam", Balance = 700, Bet = 20 });
            }
            if (num >= 4)
            {
                game.bots.Add(new Bot { Name = "Bill", Balance = 560, Bet = 20 });
            }
            if (num >= 5)
            {
                game.bots.Add(new Bot { Name = "Joker", Balance = 580, Bet = 20 });
            }
        }



        public void PlayerSave()
        {
            PlayerDAL playerDAL;
            if (!(Database.Entities.CheckValidNickname(game.Player.Name)))
            {
                playerDAL = new PlayerDAL()
                {
                    Id = game.Player.Id,
                    Name = game.Player.Name,
                    Balance = game.Player.Balance,
                    Password = game.Player.Password
                };
                Database.Entities.SaveChangePlayer(playerDAL, game.Player.Name);
            }
            if (Database.Entities.CheckValidNickname(game.Player.Name))
                {
                playerDAL = new PlayerDAL()
                {
                    Name = game.Player.Name,
                    Balance = game.Player.Balance,
                    Password = game.Player.Password
                };
                Database.Entities.SaveChangePlayer(playerDAL, game.Player.Name);
            }

        }

        public void BotsSave()
        {
            foreach (Bot bot in game.bots)
            {
                BotDAL botSaves = new BotDAL()
                {
                    BotWon = bot.BotWon,
                    BotDraw = bot.BotDraw,
                    Balance = bot.Balance,
                    Bet = bot.Bet,
                    Name = bot.Name
                };
                Database.Entities.SaveChangeBot(botSaves);
            }
        }

        public void DealerSave()
        {
            DealerDAL dealerDAL = new DealerDAL()
            {
                Id = game.Dealer.Id
            };
            Database.Entities.SaveChangeDealer(dealerDAL);
        }

        public void GameSave()
        {

                GameDAL gameDAL = new GameDAL()
                {
                    Id = game.GameId,
                    Bet = game.Bet,
                    PlayerWon = game.PlayerWon,
                    PlayerDraw = game.PlayerDraw,
                };
            Database.Entities.SaveChangeGame(gameDAL);
        }


        public String HashPassword(String password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public Player VerifyHashedPassword(String login, String password)//+
        {
            var account = Database.Entities.CheckAccountAccess(login, HashPassword(password));

            return account != null ? new Player { Id = account.Id, Name = account.Name, Balance = account.Balance, Password = account.Password } : null;
        }

        public Boolean CheckLogin(String input)
        {
           return Database.Entities.CheckValidNickname(input);
        }

        public List<Card> GetPlayerCards()
        {
            return game.Player.hand;
        }
        public List<Card> GetDealerCards()
        {
            return game.Dealer.hand;
        }
        public List<List<Card>> GetBotsCards()
        {
            List<List<Card>> cards = null;
            foreach(Bot bot in game.bots)
            {
                cards[bot.Id] = bot.hand;
            }
            return cards;
        }


        public List<String> GetListPlayers()
        {
            return Database.Entities.GetAllPlayer().ToList();
        }

        public void DeletePlayer(String login, String password)//+
        {
            Database.Entities.Delete(login, HashPassword(password));
        }


        public Boolean CheckPlayerWon()//+
        {
            return game.LastState == GameState.PlayerWon ? true : false;
        }
        public Boolean CheckPlayerLose()
        {
            return game.LastState == GameState.PlayerLose ? true : false;
        }
        public Boolean CheckPlayerDraw()
        {
            return game.LastState == GameState.Draw ? true : false;
        }


        public Boolean CheckPlayerStatus()
        {
            return (game.LastState == GameState.PlayerWon || game.LastState == GameState.PlayerLose || game.LastState == GameState.Draw) ? true : false;
        }

        public Int32 TotalValue(List<Card> hand)
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




        public void GoldBlackJackPlayerCheckup(List<Card> dealer, List<Card> player)
        {
            if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && !(player[0].CardFace == CardFaceEnum.Ace && player[1].CardFace == CardFaceEnum.Ace) && (game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.LastState = GameState.Draw;
            }
            if ((player[0].CardFace == CardFaceEnum.Ace && player[1].CardFace == CardFaceEnum.Ace) && !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance += game.Bet;
                game.LastState = GameState.PlayerWon;
            }
            if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Bet;
                game.LastState = GameState.PlayerLose;
            }
        }

        public void GoldBlackJackBotCheckup(List<Card> dealer, List<Bot> bots)
        {
            foreach (Bot bot in bots)
            {
                if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && (bot.hand[0].CardFace == CardFaceEnum.Ace && bot.hand[1].CardFace == CardFaceEnum.Ace) && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.BotState = BotState.BotDraw;
                }
                if ((bot.hand[0].CardFace == CardFaceEnum.Ace && bot.hand[1].CardFace == CardFaceEnum.Ace) && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    bot.BotState = BotState.BotWon;
                }
                if ((dealer[0].CardFace == CardFaceEnum.Ace && dealer[1].CardFace == CardFaceEnum.Ace) && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    bot.BotState = BotState.BotLose;
                }
            }
        }

        public void GameDeal()
        {
            game.LastState = GameState.Unknown;
            if (game.deck == null)
            {
                game.deck = new Deck();
            }
            if (game.deck != null)
            {
                game.deck.CardInitialize();
                game.deck.Shuffle();
            }

            foreach (Bot bot in game.bots)
            {
                bot.hand.Clear();
                bot.hand.Add(game.deck.DrowACard());
                bot.hand.Add(game.deck.DrowACard());
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
                if (TotalValue(bot.hand) == 21 & TotalValue(game.Dealer.hand) == 21 & !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.BotState = BotState.BotDraw;
                }
                if (TotalValue(bot.hand) == 21 & TotalValue(game.Dealer.hand) != 21 & !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    bot.BotState = BotState.BotWon;
                }
                if (TotalValue(bot.hand) != 21 & TotalValue(game.Dealer.hand) == 21 & !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    bot.BotState = BotState.BotLose;
                }

            }

            if (TotalValue(game.Player.hand) == 21 & TotalValue(game.Dealer.hand) == 21 & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.LastState = GameState.Draw;
                game.AllowedActions = GameAction.Deal;
            }
            if (TotalValue(game.Player.hand) == 21 & TotalValue(game.Dealer.hand) != 21 & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance += game.Bet;
                game.LastState = GameState.PlayerWon;
                game.AllowedActions = GameAction.Deal;
                BotAndDealerPlay();
            }
            if (TotalValue(game.Player.hand) != 21 & TotalValue(game.Dealer.hand) == 21 & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Bet;
                game.LastState = GameState.PlayerLose;
                game.AllowedActions = GameAction.Deal;
            }
            if (TotalValue(game.Player.hand) != 21 & TotalValue(game.Dealer.hand) != 21 & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
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
                }
            }
        }

        public void PlayerHitGame()
        {
            game.Player.hand.Add(game.deck.DrowACard());

            if (TotalValue(game.Player.hand) > 21 & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Bet;
                game.LastState = GameState.PlayerLose;
                BotAndDealerPlay();
            }
        }

        public Int32 CheckBalance()
        {
            return game.Player.Balance;
        }

        public void PlayerStandGame()
        {

            BotPlaying();

            while (TotalValue(game.Dealer.hand) < 17)
            {
                game.Dealer.hand.Add(game.deck.DrowACard());
            }



            if (TotalValue(game.Dealer.hand) > 21 || TotalValue(game.Player.hand) > TotalValue(game.Dealer.hand) & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance += game.Bet;
                game.LastState = GameState.PlayerWon;
            }
            if (TotalValue(game.Dealer.hand) == TotalValue(game.Player.hand) & !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.LastState = GameState.Draw;
            }
            if (TotalValue(game.Dealer.hand) > 21 && TotalValue(game.Player.hand) > 21 && !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.LastState = GameState.Draw;
            }
            if (TotalValue(game.Dealer.hand) <= 21 && TotalValue(game.Dealer.hand) > TotalValue(game.Player.hand) && !(game.LastState == GameState.Draw && game.LastState == GameState.PlayerWon && game.LastState == GameState.PlayerLose))
            {
                game.Player.Balance -= game.Bet;
                game.LastState = GameState.PlayerLose;
            }
            foreach (Bot bot in game.bots)
            {
                if (TotalValue(game.Dealer.hand) > 21 && TotalValue(bot.hand) <= 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    bot.BotState = BotState.BotWon;
                }
                if ((TotalValue(bot.hand) > TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && TotalValue(game.Dealer.hand) < 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    bot.BotState = BotState.BotWon;
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) > 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.BotState = BotState.BotDraw;
                }
                if ((TotalValue(bot.hand) == TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.BotState = BotState.BotDraw;
                }
                if ((TotalValue(bot.hand) < TotalValue(game.Dealer.hand)) && (TotalValue(game.Dealer.hand) <= 21 && TotalValue(bot.hand) < 21) && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    bot.BotState = BotState.BotLose;
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) <= 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    bot.BotState = BotState.BotLose;
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
                if (TotalValue(game.Dealer.hand) > 21 && TotalValue(bot.hand) <= 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    bot.BotState = BotState.BotWon;
                }
                if ((TotalValue(bot.hand) > TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && TotalValue(game.Dealer.hand) < 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance += bot.Bet;
                    bot.BotState = BotState.BotWon;
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) > 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.BotState = BotState.BotDraw;
                }
                if ((TotalValue(bot.hand) == TotalValue(game.Dealer.hand)) && TotalValue(bot.hand) <= 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.BotState = BotState.BotDraw;
                }
                if ((TotalValue(bot.hand) < TotalValue(game.Dealer.hand)) && (TotalValue(game.Dealer.hand) <= 21 && TotalValue(bot.hand) < 21) && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    bot.BotState = BotState.BotLose;
                }
                if (TotalValue(bot.hand) > 21 && TotalValue(game.Dealer.hand) <= 21 && !(bot.BotState == BotState.BotWon && bot.BotState == BotState.BotDraw && bot.BotState == BotState.BotLose))
                {
                    bot.Balance -= bot.Bet;
                    bot.BotState = BotState.BotLose;
                }
            }
        }
    }
}
