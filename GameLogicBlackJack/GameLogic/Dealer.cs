using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enum;

namespace GameLogicBlackJack.GameLogic
{

    public class Dealer : IDealer
    {
        public List<Card> dealerHand = new List<Card>();
        private String nameOfDealer;
        readonly Int32 idOfDealer;


        public Dealer(Int32 idOfDealer)
        {
            this.idOfDealer = idOfDealer;
        }

        public Int32 DealerId {
            get
            {
                return idOfDealer;
            }
        }


        public Int32 TotalValue
        {
            get
            {
                Int32 totalValue = 0;
                foreach (Card card in dealerHand)
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
            dealerHand.Clear();
        }

    }
}
