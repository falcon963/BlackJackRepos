using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enum;

namespace GameLogicBlackJack.GameLogic
{

    public class Bot : BaseHand, IBot
    {
        private String botName;
        readonly Int32 botId;

        public Bot(Int32 botId)
        {
            this.botId = botId;
            hand = new Hand(isBot: true);
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

       
    }
}
