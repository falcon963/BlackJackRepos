using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.BusinessLogic.Interface
{
    public interface IPlayerAccount
    {
        String PlayerName { get; set; }
        Decimal PlayerBalance { get; set; }
        Int32 PlayerId { get; }
        Decimal PlayerBet { get; set; }
    }
}
