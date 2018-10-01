using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SQLite;
using System.ComponentModel.DataAnnotations;
using SQLite.Net.Attributes;

namespace GameLogicBlackJack.DataAccess.Entities
{
    public class BaseEntities
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Id { get; set; }

        public BaseEntities()
        {

        }
    }
}
