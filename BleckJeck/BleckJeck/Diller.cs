using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Diller: IDiller
    {
        private Int32 id;
        

        public Int32 IdDiller{ get { return id; } set { id = value; } }
    }
}
