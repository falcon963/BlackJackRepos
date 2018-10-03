using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.BusinessLogic.Enums
{
    public enum BotState : byte
    {
        Unknown = 0,
        BotWon = 1,
        BotLose = 2,
        BotDraw = 3,
    }
}
