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

        public void DrowACard(Hand hand)
        {

            if (cards.Count <= 0)
            {
                this.Populate();
            }
            Card cardToReturn = cards[random.Next(cards.Count - 1)];
            hand.AddCard(cardToReturn);
            cards.Remove(cardToReturn);
            cardToReturn = cards[random.Next(cards.Count - 1)];
            hand.AddCard(cardToReturn);
            cards.Remove(cardToReturn);
        }

        public void GiveAdditionalCard(Hand hand)
        {
            if (cards.Count < 1)
            {
                throw new InvalidOperationException();
            }
            Card cardToReturn = cards[random.Next(cards.Count - 1)];
            hand.AddCard(cardToReturn);
            cards.RemoveAt(0);
        } 

        public Int32 GetAmountOfRemeiningCard()
        {
            return cards.Count;
        }
    }
}
