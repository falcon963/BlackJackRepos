using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite.Net.Attributes;


namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("GamesInfo")]
    public class GameDAL : BaseEntities
    {
        public GameDAL() : base() { }

        List<BotSaves> BotSaves { get; set; }
        [Required, Indexed]
        public Int32 PlayerId { get; set; }
        [Required, Indexed]
        public Int32 DealerId { get; set; }
        [Column("PlayerBet")]
        [Required]
        public Int32 Bet { get; set; }
        [Column("PlayerWin")]
        [Required]
        public Boolean PlayerWon { get; set; }
        [Column("PlayerDraw")]
        [Required]
        public Boolean PlayerDraw { get; set; }
    }
}
