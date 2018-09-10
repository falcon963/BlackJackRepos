using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    interface IBot
    {
        String BotName { get; set; }
        Int32 BotNumber { get; set; }
    }
}
