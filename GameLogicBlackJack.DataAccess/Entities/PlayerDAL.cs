using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SQLite.Net.Attributes;

namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("PlayersInfo")]
    public class PlayerDAL : BaseEntities
    {
        public PlayerDAL() : base() { }

        [Column("Balance")]
        [Required]
        public Int32 Balance { get; set; }
        
        [Column("Name")]
        [Required]
        public String Name { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
