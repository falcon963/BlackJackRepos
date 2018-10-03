using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("BotsInfo")]
    public class BotDAL : BaseEntities
    {
        public Int32 Balance { get; set; }
        public Int32 Bet { get; set; }
        public String Name { get; set; }
        public BotDAL() : base() { }
        [Column("Win")]
        [Required]
        public Boolean BotWon { get; set; }
        [Column("Draw")]
        [Required]
        public Boolean BotDraw { get; set; }
        [Required, ForeignKey("Game")]
        public Int32 GameId { get; set; }
        public GameDAL Game { get; set; }
    }
}
