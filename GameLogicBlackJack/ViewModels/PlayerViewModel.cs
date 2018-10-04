using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        public String Name { get; set; }

        public Int32 Balance { get; set; }

        public String Password { get; set; }
    }
}
