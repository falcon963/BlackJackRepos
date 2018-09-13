using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.GameLogic
{

    public class Dealer : IHand, IId
    {
        public List<Card> dealerHand = new List<Card>();
        readonly Int32 idOfDealer;


        public Dealer(Int32 idOfDealer)
        {
            this.idOfDealer = idOfDealer;
        }

        public Int32 Id {
            get
            {
                return idOfDealer;
            }
        }

        public List<Card> Hand
        {
            get
            {
                return dealerHand;
            }
            set
            {
                dealerHand = value;
            }
        }


        public void Clear()
        {
            dealerHand.Clear();
        }

    }
}
