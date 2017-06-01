using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library
{
    public static class InputHandler
    {
        public static void MovePlayer(Player player, Map map, Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    //TODO
                    break;
                case Keys.A:
                    if(player.X > 0) player.X -= player.Speed;
                    break;
                case Keys.S:
                    //TODO
                    break;
                case Keys.D:
                    if(player.X + Player.Width < Map.TileSize * (int) map.MapSize) player.X += player.Speed;
                    break;
            }
        }
    }
}
