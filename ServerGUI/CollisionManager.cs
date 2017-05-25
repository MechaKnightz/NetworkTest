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

            float tempArea = 0;

            for (int i = 0; i < world.Circles.Count; i++)
            {
                if (circle.Intersect(world.Circles[i]))
                {
                    tempArea += CircleIntersectArea(circle, world.Circles[i]);
                }
            }

            var circleArea = Math.PI * Math.Pow(circle.Radius, 2f);
            if (tempArea > circleArea)
            {
                return false;
            }

            return true;
        }

        public static float CircleIntersectArea(Circle circle, Circle other)
        {

            float r = circle.Radius;
            float R = other.Radius;
            float d = Vector2.Distance(new Vector2(circle.X, circle.Y),
                new Vector2(other.X, other.Y));
            if (R < r)
            {
                r = other.Radius;
                R = circle.Radius;
            }
            float part1 = r * r * (float)Math.Acos((d * d + r * r - R * R) / (2 * d * r));
            float part2 = R * R * (float)Math.Acos((d * d + R * R - r * r) / (2 * d * R));
            float part3 = 0.5f * (float)Math.Sqrt((-d + r + R) * (d + r - R) * (d - r + R) * (d + r + R));

            return part1 + part2 - part3;
        }
    }
}
