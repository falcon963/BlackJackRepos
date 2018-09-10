using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enum;

namespace GameLogicBlackJack.GameLogic
{
   
    public class Player : IPlayerAccount
    {
        public List<Card> playerHand = new List<Card>();
        private String nameOfPlayer;
        public Decimal balanceOfPlayer;
        private Decimal betOfPlayer;
        public readonly Int32 idOfPlayer;



        public Player( Int32 idOfPlayer)
        {
            this.idOfPlayer = idOfPlayer;
        }

        public String PlayerName {
            get
            {
                return nameOfPlayer;
            }
            set
            {
                nameOfPlayer = value;
            }
        }

        public Int32 PlayerId
        {
            get
            {
                return idOfPlayer;
            }
        }

        public Decimal PlayerBalance
        {
            get
            {
                return balanceOfPlayer;
            }
            set
            {
                balanceOfPlayer = value;
            }
        }

        public Decimal PlayerBet
        {
            get
            {
                return betOfPlayer;
            }
            set
            {
                if(betOfPlayer == value)
                {
                    return;
                }

                if(value > balanceOfPlayer + betOfPlayer && balanceOfPlayer > 0)
                {
                    betOfPlayer += balanceOfPlayer;
                    PlayerBalance = 0;
                }

                if(value < 0 && betOfPlayer > 0)
                {
                    var temp = betOfPlayer + balanceOfPlayer;
                    betOfPlayer = 0;
                    PlayerBalance = temp;
                }

                if(value >= 0 && value <= balanceOfPlayer + betOfPlayer)
                {
                    var temp = balanceOfPlayer + betOfPlayer;
                    betOfPlayer = value;
                    PlayerBalance = temp - value;
                }
            }
        }

        public Int32 TotalValue
        {
            get
            {
                Int32 totalValue = 0;
                foreach (Card card in playerHand)
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
        }

        public void Clear()
        {
            playerHand.Clear();
        }

    }
}
