using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GameLogicBlackJack.DataAccess.Entities
{
    [Table("DealersInfo")]
    public class DealerDAL : BaseEntities
    {
        public DealerDAL() : base() { }
        public virtual List<GameDAL> Game { get; set; }
    }
}
