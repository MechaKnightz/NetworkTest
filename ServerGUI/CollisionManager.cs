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
        public static bool CheckCollision(Player player, List<Player> roomPlayers)
        {
            var circle = new Circle(player.Radius, player.X, player.Y);

            foreach (var worldPlayer in roomPlayers)
            {
                if(worldPlayer.Username == player.Username) continue;
               
                var otherCircle = new Circle(worldPlayer.Radius, worldPlayer.X, worldPlayer.Y);

                if (circle.Intersect(otherCircle)) return true;
            }
            return false;
        }
    }
}
