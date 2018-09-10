using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;

namespace GameLogicBlackJack.GameLogic
{
    public class ConsoleGame
    {
        Game game = new Game(1);

        public void ConsoleChose() {
            while (true) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if ((game.AllowedActions & GameAction.Deal) == GameAction.Deal)
                    {
                        game.Deal();
                    }
                    if((game.AllowedActions & GameAction.Deal) != GameAction.Deal)
                    {
                        game.Stand();
                    }
                    break;
                }
                if(key.Key == ConsoleKey.Spacebar)
                {
                    if ((game.AllowedActions & GameAction.Deal) == GameAction.Deal)
                    {
                        game.Deal();
                    }
                    if ((game.AllowedActions & GameAction.Deal) != GameAction.Deal)
                    {
                        game.Hit();
                    }
                    break;
                }
                }
        }
        public void ConsoleBotsChoise(Bot bot)
        {
            String input = Console.ReadLine().Trim().Replace(" ", "");
            if (Convert.ToInt32(input) < 0 || Convert.ToInt32(input) > 5)
            {
                input = Console.ReadLine().Trim().Replace(" ", "");
            }
            if(Convert.ToInt32(input) == 1)
            {
                bot = new Bot(1);
            }
        }
    }
}
