using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;

namespace GameLogicBlackJack.GameLogic
{
    public class Player : BaseHand, IPlayerAccount
    {
        private String nameOfPlayer;
        private Decimal balanceOfPlayer;
        private Decimal betOfPlayer;
        public readonly Int32 idOfPlayer;


        public Player( Int32 idOfPlayer)
        {
            this.idOfPlayer = idOfPlayer;
            this.hand = new Hand(isDealer: false);
            this.hand = new Hand(isBot: false);
        }

        public String PlayerName {
            get
            {
                return nameOfPlayer;
            }
            set
            {
                nameOfPlayer = value;
            }
        }

        public Int32 PlayerId
        {
            get
            {
                return idOfPlayer;
            }
        }

        public Decimal PlayerBalance
        {
            get
            {
                return balanceOfPlayer;
            }
            set
            {
                balanceOfPlayer = value;
            }
        }

        public Decimal PlayerBet
        {
            get
            {
                return betOfPlayer;
            }
            set
            {
                if(betOfPlayer == value)
                {
                    return;
                }

                if(value > balanceOfPlayer + betOfPlayer && balanceOfPlayer > 0)
                {
                    betOfPlayer += balanceOfPlayer;
                    PlayerBalance = 0;
                }

                if(value < 0 && betOfPlayer > 0)
                {
                    var temp = betOfPlayer + balanceOfPlayer;
                    betOfPlayer = 0;
                    PlayerBalance = temp;
                }

                if(value >= 0 && value <= balanceOfPlayer + betOfPlayer)
                {
                    var temp = balanceOfPlayer + betOfPlayer;
                    betOfPlayer = value;
                    PlayerBalance = temp - value;
                }
            }
        }

    }
}
