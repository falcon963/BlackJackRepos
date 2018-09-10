using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Program
    {
        static void Main()
        {
            Deck deck = new Deck();
            NewGame game = new NewGame();
            //game.GetDiller();
            //game.GetUser();
          //GameLogic.Hand();
            for(int i = 0; i < 20; i++) {
                GameLogic.Hand();
                i++;
            }
        }
    }
 
}



