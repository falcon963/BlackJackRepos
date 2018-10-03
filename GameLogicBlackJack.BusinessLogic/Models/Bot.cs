using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.BusinessLogic.Enums;

namespace GameLogicBlackJack.BusinessLogic.Models
{

    public class Bot : BaseModel, IBet, IBalance, IPlayers
    {

        public String Name { get; set; }

        public Int32 Balance { get; set; }

        public Int32 Bet { get; set; }

        public Boolean BotWon { get; set; }
        public Boolean BotDraw { get; set; }

        public BotState BotState { get; set; }

        public Bot()
        {
            this.BotState = BotState.Unknown;
        }
    }
}
