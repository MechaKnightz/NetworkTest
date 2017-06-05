using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library
{
    public class GameRoom
    {
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public Map Map { get; set; }

        public GameRoom(string name)
        {
            Name = name;
            Players = new List<Player>();
            Map = new Map(MapSize.Medium);
        }

        public GameRoom()
        {
            Players = new List<Player>();
            Map = new Map(MapSize.Medium);
        }

        public void GravityMove(Player player)
        {
            InputHandler.MoveWithCollisionCheck(new Vector2(0, GlobalConsts.GravityConst), player, Map);
        }

        public void HandleInput(Player player, Keys key)
        {
            InputHandler.MovePlayer(player, Map, key);
        }

        public static void GravityMovePlayers(List<GameRoom> gameRooms)
        {
            for (int i = 0; i < gameRooms.Count; i++)
            {
                for (int j = 0; j < gameRooms[i].Players.Count; j++)
                {
                    gameRooms[i].GravityMove(gameRooms[i].Players[j]);
                }
            }
        }
    }
}
