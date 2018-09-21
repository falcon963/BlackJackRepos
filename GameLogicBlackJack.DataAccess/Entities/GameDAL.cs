using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;


namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("GamesInfo")]
    public class GameDAL : BaseEntities
    {
        [Indexed]
        public Int32 PlayerId { get; set; }
        public virtual PlayerDAL Player { get; set; }
        [Indexed]
        public Int32 BotId { get; set; }
        public virtual BotDAL Bot { get; set; }
        [Indexed]
        public Int32 DealerId { get; set; }
        public virtual DealerDAL Dealer { get; set; }
    }
}
