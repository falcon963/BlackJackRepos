using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;


namespace GameLogicBlackJack.GameLogic
{
    public class Hand
    {
        Deck _deck;
        public List<Card> cards = new List<Card>();

        public Hand(Boolean isDealer = false, Boolean isBot = false)
        {
            IsDealer = isDealer;
            IsBot = isBot;  
        }

        public Boolean IsDealer { get; set; }
        public Boolean IsBot { get; set; }

        public EventHandler Change;


        public Int32 TotalValue
        {
            get
            {
                Int32 totalValue = 0;
                foreach(Card card in cards)
                {
                    if((Int32)card.CardFace > 1 & (Int32)card.CardFace < 11)
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

        
        public Boolean IsBlackJack { get; set; }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public void Clear()
        {
            cards.Clear();
        }

    }
}
