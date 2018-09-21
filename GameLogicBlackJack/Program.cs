using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Models;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.DataAccess.SQLite;

namespace GameLogicBlackJack
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var db = new BlackJackContext();
            db.DataBaseCreateTable();
            GameController controllers = new GameController();
            controllers.GameInitialize();
            controllers.ConsoleChoise();
            Console.WriteLine("Game End");
            Console.ReadKey();
        }
    }
}
