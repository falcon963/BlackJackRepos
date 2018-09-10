using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class NewGame
    {
        User player1 = new User();
        Diller diller1 = new Diller();
        
        public void GetDiller()
        {
            diller1.IdDiller = 1;
        }

        public void GetUser()
        {
            Int32 newBalance = 0;
            Console.WriteLine("Enter nickname: ");
            String inputL = Console.ReadLine().Trim().Replace(" ", "");
            player1.PlayerName = inputL;
            Console.WriteLine("Enter sum:");
            inputL = Console.ReadLine().Trim().Replace(" ", "");
            Int32.TryParse(inputL, out newBalance);
            if (newBalance < 0)
            {
                Console.WriteLine("Enter sum:");
                inputL = Console.ReadLine().Trim().Replace(" ", "");
            }
        }
    }
}
