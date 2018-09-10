using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;

namespace GameLogicBlackJack.GameLogic
{
    public class Dealer : BaseHand, IDealer
    {
        private String nameOfDealer;
        readonly Int32 idOfDealer;


        public Dealer(Int32 idOfDealer)
        {
            this.idOfDealer = idOfDealer;
            hand = new Hand(isDealer: true);
        }

        public Int32 DealerId {
            get
            {
                return idOfDealer;
            }
        }

        public String DealerName
        {
            get
            {
                return nameOfDealer;
            }
            set
            {
                nameOfDealer = value;
            }
        }

     



    }
}
