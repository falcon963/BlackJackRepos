using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.BusinessLogic.Interface
{
    public interface IBot
    {
        String BotName { get; set; }
        Int32 BotId { get; }
    }
}
