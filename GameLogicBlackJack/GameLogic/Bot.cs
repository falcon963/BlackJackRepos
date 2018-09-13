using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.GameLogic
{

   

    public class Bot : IHand, IBet, IBalance, IId
    {
        public List<Card> botHand = new List<Card>();
        readonly Int32 botId;
        public Decimal botBalance = 500;
        public Decimal botBet;

        public List<Card> Hand
        {
            get
            {
                return botHand;
            }
            set
            {
                botHand = value;
            }
        }

        public Bot(Int32 botId)
        {
            this.botId = botId;
        }

        public Decimal Balance
        {
            get
            {
                return botBalance;
            }
            set
            {
                botBalance = value;
            }
        }

        public Decimal Bet
        {
            get
            {
                return botBet;
            }
            set
            {
                if(botBalance < 500)
                {
                    botBet = 20;
                }
                if(botBalance < 100)
                {
                    botBet = 10;
                }
                botBet = 50;
            }
        }


        public Int32 Id
        {
            get
            {
                return botId;
            }
        }


        public void Clear()
        {
            botHand.Clear();
        }

    }
}
