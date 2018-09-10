using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.GameLogic;

namespace GameLogicBlackJack
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Game game = new Game();
            for (int i = 0; i < 10; i++)
                game.NewGame();
                Console.WriteLine("End game!");
                Console.ReadKey();
        }
    }
}
