using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.Controllers;
using GameLogicBlackJack.BusinessLogic.Services;
using GameLogicBlackJack.BusinessLogic.Interface;
using GameLogicBlackJack.Services;

namespace GameLogicBlackJack
{
    class Program
    {
        
        static void Main(string[] args)
        {  
            UIService.GetInstance().controller.Launcher();
            Console.WriteLine("...");
            Console.ReadKey();
        }
    }
}
