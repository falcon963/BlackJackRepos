using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.Models
{
   
    public class Player : BaseModel, IPlayers, IBet, IBalance
    {

        public String Name { get; set; }

        public Int32 Balance { get; set; }

        public Int32 Bet { get; set; }

        public Boolean PlayerWon { get; set; }
        public Boolean PlayerDraw { get; set; }
    }
}
