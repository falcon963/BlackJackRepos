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

        public Decimal Balance { get; set; }

        public Decimal Bet { get; set; }

        public Boolean PlayerWon { get; set; }
    }
}
