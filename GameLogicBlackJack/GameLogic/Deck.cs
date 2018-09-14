using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;


namespace GameLogicBlackJack.GameLogic
{
    public class Deck
    {
        Random random = new Random();
        public List<Card> cards = new List<Card>();
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
            this.Populate();
        }

        public void Populate()
        {
            cards.Clear();
          //  cards.AddRange(Enumerable.Range(1, GetEnumEntries<CardSuitEnum>()).SelectMany(s => Enumerable.Range(1, GetEnumEntries<CardFaceEnum>()).Select(n => new Card((CardFaceEnum)n, (CardSuitEnum)s))));
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
            Random rng = new Random();
            int n = cards.Count;
            for(Int32 i = 1; n > i ;)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }

        public Card DrowACard()
        {

            if (cards.Count <= 0)
            {
                this.Populate();
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
