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
                    player.VelocityX = -player.Speed;
                    break;
                case Keys.S:
                    //TODO
                    break;
                case Keys.D:
                    player.VelocityX = player.Speed;
                    break;
                case Keys.Space:
                    Jump(player, map);
                    break;
            }
        }

        //true of moved, false if not
        public static bool MoveWithCollisionCheck(Vector2 offset, Player player, Map map)
        {
            var tempPlayer = (Player)player.Clone();

            tempPlayer.X += offset.X;
            tempPlayer.Y += offset.Y;

            var tempPlayerRect = GetPlayerRectangle(tempPlayer);

            for (int i = 0; i < map.MapData.Count; i++)
            {
                for (int j = 0; j < map.MapData[i].Count; j++)
                {
                    if(map.MapData[i][j].Intersects(tempPlayerRect, j, i)) return false;
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
                    if (map.MapData[i][j].MouseIntersect(x, y, j, i))
                    {
                        //TODO make list dirty if somehting is changed
                        map.Dirty = true;
                        map.MapData[i][j].OnLeftClick();
                        if (map.MapData[i][j].Health <= 0)
                        {
                            map.MapData[i][j] = new Air();
                            map.MapData[i][j].Dirty = true;
                        }
                    }
                }
            }
        }

        public static void RightClick(List<Player> allPlayers, Player player, Map map, float x, float y)
        {
            for (int i = 0; i < map.MapData.Count; i++)
            {
                for (int j = 0; j < map.MapData[i].Count; j++)
                {
                    if (map.MapData[i][j].MouseIntersect(x, y, j, i) && map.MapData[i][j].Id == TileType.Air)
                    {
                        if(IsAPlayerInTile(allPlayers, map.MapData[i][j], i, j)) return;
                        map.MapData[i][j] = new Dirt();
                        map.MapData[i][j].Dirty = true;
                        map.Dirty = true;
                    }
                }
            }
        }

        public static void MiddleClick(Player player, Map map, float x, float y)
        {

        }

        public static void Jump(Player player, Map map)
        {
            //https://jsfiddle.net/LyM87
            if (player.OnGround && !player.IsJumping)
            {
                player.VelocityY = -Player.JumpStartVelocity;
                player.IsJumping = true;
            }
        }

        public static bool MoveWithAdjust(Vector2 offset, Player player, Map map)
        {
            //todo refactor into MoveWithCollisionCheck method
            //wow dis be innefficient
            //todo
            bool returnEarly = false;
            bool booler = !MoveWithCollisionCheck(offset, player, map);
            if (!booler) return true;
            if (offset.Y > 0)
            {
                for (int i = (int)offset.Y; i >= 1; i--)
                {
                    if (MoveWithCollisionCheck(new Vector2(0, i), player, map))
                        returnEarly = true;
                }
            }
            else if(offset.Y < 0)
            {
                for (int i = (int)offset.Y; i <= -1; i++)
                {
                    if (MoveWithCollisionCheck(new Vector2(0, i), player, map))
                        returnEarly = true;
                }
            }

            if (offset.X > 0)
            {
                for (int i = (int)offset.X; i >= 1; i--)
                {
                    if (MoveWithCollisionCheck(new Vector2(i, 0), player, map))
                        returnEarly = true;
                }
            }
            else if (offset.X < 0)
            {
                for (int i = (int)offset.X; i <= -1; i++)
                {
                    if (MoveWithCollisionCheck(new Vector2(i, 0), player, map))
                        returnEarly = true;
                }
            }
            if (returnEarly) return false;

            return false;
        }

        public static bool IsAPlayerInTile(List<Player> allPlayers, ITile tile, int row, int column)
        {
            var rect = new Rectangle(column * Map.TileSize, row * Map.TileSize, Map.TileSize, Map.TileSize);

            for (int i = 0; i < allPlayers.Count; i++)
            {
                var tempRect = GetPlayerRectangle(allPlayers[i]);
                if (rect.Intersects(tempRect)) return true;
            }
            return false;
        }

        public static Rectangle GetPlayerRectangle(Player player)
        {
             return new Rectangle(
                Convert.ToInt32(player.X),
                Convert.ToInt32(player.Y),
                Convert.ToInt32(Player.Width),
                Convert.ToInt32(Player.Height));
        }
    }
}
