using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLogicBlackJack.MVC.Models
{
    public class PlayerData
    {
        public Int32 Id { get; set; }
        public Decimal Balance { get; set; }
        public String NickName { get; set; }
    }
}
