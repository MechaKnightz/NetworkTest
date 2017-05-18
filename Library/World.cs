using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class World
    {
        public World(List<Player> players)
        {
            Players = players;
        }
        public List<Circle> Circles { get; set; } = new List<Circle>();
        public List<Player> Players { get; set; }
    }
}
