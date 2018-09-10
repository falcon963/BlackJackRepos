using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    interface IGame : IDiller, IPlayerAcount, IBot
    {
        Int32 GameId { get; set; }
        Int32 GameBank { get; set; }
        Int32 NumberOfPlayers { get; set; }
        //String GameWinner { get; set; }
    }
}
