using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace GameLogicBlackJack.DataAccess.Entities
{
    public class BotSaves : BotDAL
    {
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

        ICollection<GameDAL> GameDAL { get; set; }
    }
}
