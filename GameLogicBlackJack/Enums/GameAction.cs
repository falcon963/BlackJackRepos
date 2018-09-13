using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.Enums
{
    [Flags]
    public enum GameAction : Byte
    {
        None = 1,
        Deal = 2,
        Stand = 4,
        Hit = 8
    }
}
