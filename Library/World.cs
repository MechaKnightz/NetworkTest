using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Messenger;

namespace Library
{
    public class World
    {
        public World() { }
        public List<Circle> Circles { get; set; } = new List<Circle>();
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Message> ChatMessages { get; set; } = new List<Message>();
    }
}
