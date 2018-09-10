using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    
    public class Deck
    {
        Random random = new Random();
        private List<AllCards> cards;
        public Deck()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            cards = new List<AllCards>();
            for (Int32 i = 0; i < 4; i++)
            {
                for (Int32 j = 0; j < 13; j++)
                {
                    cards.Add(new AllCards() { Suit = (Suit)i, Face = (Face)j });

                    if (j <= 8)
                        cards[cards.Count - 1].Value = j + 1;
                    else
                        cards[cards.Count - 1].Value = 10;
                }
            }
        }
       
        /* public void Shiffer()
         {
             Random rng = new Random();
             Int32 n = cards.Count;
             while(n > 1){
                 n--;
                 Int32 k = rng.Next(n + 1);
                 AllCards card = cards[k];
                 cards[k] = cards[n];
                 cards[n] = card;
             }
    }*/
        public AllCards DrowACard()
        {
           
            if(cards.Count <= 0)
            {
                this.Initialize();
              
            }
            AllCards cardToReturn = cards[random.Next(cards.Count - 1)];
            return cardToReturn;
        }
        public Int32 GetAmountOfRemeiningCard()
        {
            return cards.Count;
        }

        public void PrintDeck()
        {
            int i = 1;
            foreach (AllCards card in cards)
            {
                Console.WriteLine("Card{0}:{1} {2}. Value:{3}", i, card.Face, card.Suit, card.Value);
                i++;
            }
        }
    }
}
