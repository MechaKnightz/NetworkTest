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

        public void Intersection(Player player)
        {
            var rectangle = new Rectangle(Convert.ToInt32(player.X),
                Convert.ToInt32(player.Y),
                Convert.ToInt32(Player.Width),
                Convert.ToInt32(Player.Height));


            for (int i = 0; i < Map.MapData.Count; i++)
            {
                for (int j = 0; j < Map.MapData[i].Count; j++)
                {
                    var tempRect = Map.MapData[i][j].GetCollisionRectangle(j * Map.TileSize, i * Map.TileSize);

                    var tempRect2 = rectangle;
                    tempRect2.Y += (int)GlobalConsts.GravityConst;

                    if (tempRect.Intersects(tempRect2))
                    {
                        player.Falling = false;
                    }
                }
            }
        }

        public void HandleInput(Player player, Keys key)
        {
            InputHandler.MovePlayer(player, Map, key);
        }
    }
}
