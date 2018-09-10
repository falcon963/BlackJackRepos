using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Bot : IBot
    {
        private Int32 balanceBot;
        private String nameBot;
        public Bot(Int32 balance, String name)
        {
            balanceBot = balance;
            nameBot = name;
        }

        public Int32 BotNumber { get { return balanceBot; } set { balanceBot = value; } }
       
        public String BotName { get { return nameBot; } set { nameBot = value; } }

        
        
    }
}
