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
        public static void MovePlayer(Player player, Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    player.X = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation, player.Speed).X;
                    player.Y = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation, player.Speed).Y;
                    break;
                case Keys.A:
                    player.Rotation -= 0.05f;
                    break;
                case Keys.S:
                    player.X =
                        Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation + (float)Math.PI,
                            player.Speed / 5 * 2).X;
                    player.Y =
                        Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation + (float)Math.PI,
                            player.Speed / 5 * 2).Y;
                    break;
                case Keys.D:
                    player.Rotation += 0.05f;
                    break;
            }
        }
    }
}
