using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;


namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("DealersInfo")]
    public class DealerDAL : BaseEntities
    {
        ICollection<GameDAL> Game { get; set; }
    }
}
