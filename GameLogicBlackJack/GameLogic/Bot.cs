using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enum;

namespace GameLogicBlackJack.GameLogic
{

   

    public class Bot : IBot
    {
        public List<Card> botHand = new List<Card>();
        private String botName;
        readonly Int32 botId;
        public Decimal botBalance = 500;
        public Decimal botBet;

        public Bot(Int32 botId)
        {
            this.botId = botId;
        }

        public Decimal BotBalance
        {
            get
            {
                return botBalance;
            }
            set
            {
                if(botBalance < 10)
                {
                    botBalance = 500;
                }
                if(botBalance >= 10)
                {
                    botBalance = value;
                }
            }
        }

        public Decimal BotBet
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

        public String BotName
        {
            get
            {
                return botName;
            }
            set
            {
                botName = value;
            }
        }

        public Int32 BotId
        {
            get
            {
                return botId;
            }
        }

        public Int32 TotalValue
        {
            get
            {
                Int32 totalValue = 0;
                foreach (Card card in botHand)
                {
                    if ((Int32)card.CardFace > 1 & (Int32)card.CardFace < 11)
                    {
                        card.CardValue = (Int32)card.CardFace;
                    }
                    if ((Int32)card.CardFace >= 11)
                    {
                        card.CardValue = 10;
                    }
                    if (card.CardFace == CardFaceEnum.Ace & totalValue + 11 < 22)
                    {
                        card.CardValue = 11;
                    }
                    if (card.CardFace == CardFaceEnum.Ace & totalValue + 11 > 22)
                    {
                        card.CardValue = 1;
                    }
                    totalValue += card.CardValue;
                }
                return totalValue;
            }
        }

        public void Clear()
        {
            botHand.Clear();
        }

    }
}
