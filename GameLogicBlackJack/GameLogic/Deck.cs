using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;


namespace GameLogicBlackJack.GameLogic
{
    public class Deck
    {
        Random random = new Random();
        public List<Card> cards = new List<Card>(52);
        public Deck()
        {
            this.Populate();
        }

        public void Populate()
        {
            cards.Clear();
            cards.AddRange(Enumerable.Range(1, 4).SelectMany(s => Enumerable.Range(1, 13).Select(n => new Card((CardFaceEnum)n, (CardSuitEnum)s))));
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
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
