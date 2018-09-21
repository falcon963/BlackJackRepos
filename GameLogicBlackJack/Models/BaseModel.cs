using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.Models
{
    public class BaseModel
    {
        public List<Card> hand = new List<Card>();
        public Int32 Id { get; set; }
        public List<Card> Hand { get; set; }

        public void Clear()
        {
            hand.Clear();
        }
    }
}
