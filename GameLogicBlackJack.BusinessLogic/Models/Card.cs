using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enums;

namespace GameLogicBlackJack.BusinessLogic.Models
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
