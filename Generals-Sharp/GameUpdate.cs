using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals_Sharp
{
    public class Score
    {
        public int total { get; set; }
        public int tiles { get; set; }
        public int i { get; set; }
        public bool dead { get; set; }
    }

    public class GameUpdate
    {
        public List<Score> scores { get; set; }
        public int turn { get; set; }
        public int[] stars { get; set; }
        public int attackIndex { get; set; }
        public int[] generals { get; set; }
        public int[] map_diff { get; set; }
        public int[] cities_diff { get; set; }
    }
}
