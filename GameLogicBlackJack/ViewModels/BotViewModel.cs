using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.ViewModels
{
    public class BotViewModel : BaseViewModel
    {
        public String Name { get; set; }

        public Int32 Balance { get; set; }

        public Int32 Bet { get; set; }

        public Boolean BotWon { get; set; }
        public Boolean BotDraw { get; set; }
    }
}
