using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;


namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("BotsInfo")]
    public class BotDAL : BaseEntities
    {
        
        public Int32 Balance { get; set; }
        public Int32 Bet { get; set; }
        public String Name { get; set; }
    }
}
