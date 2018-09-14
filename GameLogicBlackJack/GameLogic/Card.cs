using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.GameLogic
{
    public class Card
    {
        public Card(CardFaceEnum cardFace, CardSuitEnum cardSuit)
        {
            CardFace = cardFace;
            CardSuit = cardSuit;
        }
        public CardFaceEnum CardFace { get; set; }
        public CardSuitEnum CardSuit { get; set; }
        public Int32 CardValue { get; set; }
    }
}
