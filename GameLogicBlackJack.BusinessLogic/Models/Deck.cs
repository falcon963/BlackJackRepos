using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enums;


namespace GameLogicBlackJack.BusinessLogic.Models
{
    public class Deck
    {
        public List<Card> cards;
        public static Int32 GetEnumEntries<T>() where T: struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }
            return Enum.GetNames(typeof(T)).Length;
        }
        public Deck()
        {
            cards = new List<Card>();
            this.CardInitialize();
        }

        public void CardInitialize()
        {
            if(cards == null)
            {
                cards = new List<Card>();
            }

            cards.Clear();

            for (Int32 i = 1; i < 5; i++)
            {
                for(Int32 j = 1; j < 14; j++)
                {
                    cards.Add(new Card((CardFaceEnum)j, (CardSuitEnum)(CardFaceEnum)i));
                    if (j <= 9)
                    {
                        cards[cards.Count - 1].CardValue = j;
                    }
                    if(j > 9)
                    {
                        cards[cards.Count - 1].CardValue = 10;
                    }
                }
            }
        }

        public void Shuffle()
        {
            Random randomCard = new Random();
            int countCard = cards.Count;
            for(Int32 i = 1; countCard > i ;)
            {
                countCard--;
                int k = randomCard.Next(countCard + 1);
                Card card = cards[k];
                cards[k] = cards[countCard];
                cards[countCard] = card;
            }
        }

        public Card DrowACard()
        {

            if (cards.Count <= 0)
            {
                this.CardInitialize();
                this.Shuffle();
            }

            Card cardToReturn = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return cardToReturn;
        }

        public Int32 GetAmountOfRemeiningCard()
        {
            return cards.Count;
        }
    }
}
