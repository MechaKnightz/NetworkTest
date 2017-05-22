using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Xna.Framework;

namespace ServerGUI
{
    public class CollisionManager
    {
        public static bool CheckCollision(Player player, World world)
        {
            var circle = new Circle(player.Radius, player.X, player.Y);

            foreach (var worldPlayer in world.Players)
            {
                if(worldPlayer.Username == player.Username) continue;
               
                var otherCircle = new Circle(worldPlayer.Radius, worldPlayer.X, worldPlayer.Y);

                if (circle.Intersect(otherCircle)) return true;
            }
            return false;
        }

        public static bool CheckCollisionCircles(Player player, World world)
        {
            var circle = new Circle(player.Radius, player.X, player.Y);

            for (int i = 0; i < world.Circles.Count; i++)
            {
                if (world.Circles[i].Contains(circle)) return false;
            }
            return true;
        }
    }
}
