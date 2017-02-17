using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals_Sharp
{
    // stores the GameStart message sent by the server
    public class GameStart
    {
        public int playerIndex { get; set; }
    }
}
