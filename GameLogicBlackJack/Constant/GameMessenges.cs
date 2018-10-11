using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.Constant
{
    struct GameMessenges
    {
        public const String WrongBalanceEnter = "Wrong balance, enter again:";
        public const String WrongNumberOfBots = "Wrong number of bots, enter again:";
        public const String WrongNicknameEnter = "Wrong nickname, enter again:";
        public const String WrongPasswordEnter = "Wrong password, enter again:";
        public const String WrongBetEnter = "Wrong bet, enter again:";
        public const String AccountAccessError = "Wrong login or password! Not complete, try again!";
        public const String ExitComplitMessenge = "Exit complite.";
        public const String MenuListMessenge = "Menu:\n1|\tLoad\n2|\tPlayers list\n3|\tDelete account\n4|\tNew account\nDelete|\tClear game data\nEsc|\tExit";
        public const String PressKeyMessenge = "Press Enter if you want continue, or press Space if you want take card\n";
        public const String BalancePlayerErrorMessenge = "Insufficient funds in the account!";
        public const String StartNewGameMessenge = "Start new game - press N, stop game - press Esc\n";
        public const String GameStopMessenge = "Game was stop";
        public const String EnterBalanceMessenge = "Enter balance:";
        public const String EnterPasswordMessenge = "Enter you password:";
        public const String EnterNicknameMessenge = "Enter you nickname:";
        public const String EnterNumBots = "Enter num bots(0 - 5):";
        public const String ComleteMessenge = "Comlete!";
        public const String Line = "-------------------------------------------";
        public const String DeleteMessenge = "Do you want clear game data? Press Y(es)/N(o)";
    }
}
