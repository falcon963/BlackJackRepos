using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.UserInterface.UI
{ 
    public delegate void UI();
    public class GameEvents
    {
        public event UI PlayerInitialize;

        public void OnInitializeNickname()
        {
            PlayerInitialize();
        }
    }
}
