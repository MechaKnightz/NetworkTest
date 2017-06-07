using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Tiles;
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
                    MoveWithCollisionCheck(new Vector2(-player.Speed, 0), player, map);
                    break;
                case Keys.S:
                    //TODO
                    break;
                case Keys.D:
                     MoveWithCollisionCheck(new Vector2(player.Speed, 0), player, map);
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
            if (tempPlayer.Y + Player.Height > Map.TileSize * (int) map.MapSize) return false;
            if (tempPlayer.Y < 0) return false;
            if (tempPlayer.X + Player.Width > Map.TileSize * (int) map.MapSize) return false;
            if (tempPlayer.X < 0) return false;

            player.X += offset.X;
            player.Y += offset.Y;
            return true;
        }

        public static void LeftClick(Player player, Map map, float x, float y)
        {
            for (int i = 0; i < map.MapData.Count; i++)
            {
                for (int j = 0; j < map.MapData[i].Count; j++)
                {
                    var rect = map.MapData[i][j].GetClickRectangle(j * Map.TileSize, i * Map.TileSize);
                    if (rect.Width == 0 || rect.Height == 0) continue;
                    if (rect.Contains(x, y))
                    {
                        map.MapData[i][j].OnLeftClick();
                        if (map.MapData[i][j].Health <= 0)
                        {
                            map.MapData[i][j] = new Air();
                            map.MapData[i][j].Dirty = true;
                            map.Dirty = true;
                        }
                    }
                }
            }
        }

        public static void RightClick(Player player, Map map, float x, float y)
        {

        }

        public static void MiddleClick(Player player, Map map, float x, float y)
        {

        }
    }
}
