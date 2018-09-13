using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.GameLogic
{
   
    public class Player : IPlayerAccount, IHand, IId, IBet, IBalance
    {
        public List<Card> playerHand = new List<Card>();
        private String nameOfPlayer;
        public Decimal balanceOfPlayer;
        private Decimal betOfPlayer;
        public readonly Int32 idOfPlayer;



        public Player( Int32 idOfPlayer)
        {
            this.idOfPlayer = idOfPlayer;
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

        public Int32 Id
        {
            get
            {
                return idOfPlayer;
            }
        }

        public Decimal Balance
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

        public Decimal Bet
        {
            get
            {
                return betOfPlayer;
            }
            set
            {
                betOfPlayer = value;
            }
        }

        public List<Card> Hand
        {
            get
            {
                return playerHand;
            }
            set
            {
                playerHand = value;
            }
        }

       

        public void Clear()
        {
            playerHand.Clear();
        }

    }
}
