using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLogicBlackJack.MVC.Models
{
    public class GameData
    {
        public Int32 GameId { get; set; }
        public Int32 BotsNumber { get; set; }

        public Int32 PlayerId { get; set; }
        public PlayerData PlayerData { get; set; }
    }
}
