using System;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ServerGUI.Commands;

namespace ServerGUI.Commands
{
    class InputCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            var dirty = false;
            var dirtyPlayer = new Player();

            foreach (var player2 in world.Players)
            {
                if (player2.Conn != inc.SenderConnection)
                    continue;

                var key = (Keys)inc.ReadByte();

                ReadInput(player2, world, key);

                dirty = true;
                dirtyPlayer = player2;
            }
            if (dirty)
            {
                var command = new SendPlayerCommand();
                command.Run(loggerManager, server, inc, dirtyPlayer, world);
                return;
            }
            loggerManager.AddServerLogMessage("Couldn't find player with " + inc.SenderConnection);
        }

        private static void ReadInput(Player player, World world, Keys key)
        {
            var tempPlayer = (Player)player.Clone();

            MovePlayer(tempPlayer, key);

            if (CollisionManager.CheckCollision(tempPlayer, world))
            {
                return;
            }

            MovePlayer(player, key);
        }

        private static void MovePlayer(Player player, Keys key)
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
