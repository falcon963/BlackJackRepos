using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;


namespace GameLogicBlackJack.Models
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
