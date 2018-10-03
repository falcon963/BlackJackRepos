using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.BusinessLogic.Enums
{
    public enum GameState : Byte
    {
        Unknown = 0,
        PlayerWon = 1,
        PlayerLose = 2,
        Draw = 3
    }
}
