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
                    if(player.X > 0) MoveWithCollisionCheck(new Vector2(-player.Speed, 0), player, map);
                    break;
                case Keys.S:
                    //TODO
                    break;
                case Keys.D:
                    if(player.X + Player.Width < Map.TileSize * (int) map.MapSize) MoveWithCollisionCheck(new Vector2(player.Speed, 0), player, map);
                    break;
            }
        }

        //true of moved, false if not
        public static bool MoveWithCollisionCheck(Vector2 offset, Player player, Map map)
        {
            var tempPlayer = (Player)player.Clone();

            tempPlayer.X += offset.X;
            tempPlayer.Y += offset.Y;

            var tempPlayerRect = new Rectangle(
                Convert.ToInt32(tempPlayer.X),
                Convert.ToInt32(tempPlayer.Y),
                Convert.ToInt32(Player.Width),
                Convert.ToInt32(Player.Height));

            for (int i = 0; i < map.MapData.Count; i++)
            {
                for (int j = 0; j < map.MapData[i].Count; j++)
                {
                    var rect = map.MapData[i][j].GetCollisionRectangle(j * Map.TileSize, i * Map.TileSize);
                    if (rect.Width == 0 || rect.Height == 0) continue;
                    if (tempPlayerRect.Intersects(rect)) return false;
                }
            }

            player.X += offset.X;
            player.Y += offset.Y;
            return true;
        }
    }
}
