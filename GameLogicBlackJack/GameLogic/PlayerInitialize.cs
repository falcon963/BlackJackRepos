using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.UserInterface.UI;

namespace GameLogicBlackJack.GameLogic
{
    public class PlayerInitialize
    {
        String inputLine;
        Int32 newBalance;
        GameEvents events = new GameEvents();
        Messenger messenger = new Messenger();
        

        static Random randomId = new Random();

        PlayerAccount _player = new PlayerAccount(randomId.Next(200000, 300000));

        public String GetNickname()
        {
            events.PlayerInitialize += messenger.PlayerEnterNickname;
            events.OnInitializeNickname();
            events.PlayerInitialize -= messenger.PlayerEnterNickname;
            inputLine = ConsoleGame.ReadLine().Trim().Replace(" ", "");
            _player.PlayerName = inputLine;
            return _player.PlayerName;
        }

        public Int32 GetBalance()
        {
            events.PlayerInitialize += messenger.PlayerEnterBalance;
            events.OnInitializeNickname();
            inputLine = ConsoleGame.ReadLine().Trim().Replace(" ", "");
            Int32.TryParse(inputLine, out newBalance);
            if (newBalance < 0)
            {
                events.OnInitializeNickname();
                inputLine = ConsoleGame.ReadLine().Trim().Replace(" ", "");
            }
            events.PlayerInitialize -= messenger.PlayerEnterBalance;
            _player.PlayerBalance = newBalance;
            return _player.PlayerBalance;
        }
    }
}
