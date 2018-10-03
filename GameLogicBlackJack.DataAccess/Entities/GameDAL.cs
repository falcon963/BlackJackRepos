using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("GamesInfo")]
    public class GameDAL : BaseEntities
    {
        public GameDAL() : base() { }

        public List<BotDAL> Bots { get; set; }

        [ForeignKey("Player")]
        public Int32 PlayerId { get; set; }
        [ForeignKey("Dealer")]
        public Int32 DealerId { get; set; }
        [Column("PlayerBet")]
        public Int32 Bet { get; set; }
        [Column("PlayerWin")]
        public Boolean PlayerWon { get; set; }
        [Column("PlayerDraw")]
        public Boolean PlayerDraw { get; set; }

        public  PlayerDAL Player { get; set; }
        public virtual DealerDAL Dealer { get; set; }
    }
}
