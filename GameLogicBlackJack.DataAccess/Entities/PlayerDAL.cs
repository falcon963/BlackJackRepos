using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SQLite;

namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("PlayersInfo")]
    public class PlayerDAL : BaseEntities
    {
        [Column("Balance")]
        public Int32 Balance { get; set; }
        [Column("Bet")]
        public Int32 Bet { get; set; }
        [Column("Name")]
        [Required]
        public String Name { get; set; }
        [Required]
        public Boolean PlayerWon { get; set; }

        ICollection<GameDAL> Game { get; set; }
    }
}
