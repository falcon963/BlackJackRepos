using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GameLogicBlackJack.DataAccess.Entities
{
    public class BotSaves : BotDAL
    {
        public BotSaves() : base() { }
        [Column("Win")]
        [Required]
        public Boolean BotWon { get; set; }
        [Column("Draw")]
        [Required]
        public Boolean BotDraw { get; set; }
        [Ignore]
        public String Name { get; set; }
        [Ignore]
        public Int32 Balance { get; set; }
        [Ignore]
        public Int32 Bet { get; set; }
        [Required, Indexed]
        public Int32 GameId { get; set; }

    }
}
