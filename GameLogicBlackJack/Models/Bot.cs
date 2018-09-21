using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Interface;
using GameLogicBlackJack.Enums;

namespace GameLogicBlackJack.Models
{

    public class Bot :BaseModel, IBet, IBalance, IPlayers
    {
        
        public String Name{ get; set; }

        public Decimal Balance { get; set; }

        public Decimal Bet { get; set; }

        public Boolean BotWon { get; set; }
    }
}
