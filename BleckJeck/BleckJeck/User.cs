using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class User : IPlayerAcount
    {
        private String name;
        private Int32 balance;

        /*  public User(String nickname, Int32 balance) {
             PlayerName = nickname;
             PlayerBalance = balance;
           }*/
        public String PlayerName { get { return name; } set {  name = value;  } }
       
        public Int32 PlayerBalance { get { return balance; } set { balance = value; } }

       
        }
    } 
