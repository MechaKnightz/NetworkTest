using System;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Server.Commands
{
    class InputCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            var dirty = false;
            var dirtyPlayer = new Player();
            foreach (var player2 in world.Players)
            {
                if (player2.Conn != inc.SenderConnection)
                    continue;

                var b = inc.ReadByte();

                ReadInput(player2, b);

                dirty = true;
                dirtyPlayer = player2;
            }
            if (dirty)
            {
                var command = new SendPlayerCommand();
                command.Run(server, inc, dirtyPlayer, world);
                return;
            }
            Console.WriteLine("Couldn't find player with " + inc.SenderConnection);
        }

        private static void ReadInput(Player player, byte b)
        {
            if ((byte)Keys.D == b)
                player.Rotation += 0.05f;
            if ((byte)Keys.W == b)
            {
                player.X = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation, player.Speed).X;
                player.Y = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation, player.Speed).Y;
            }
            if ((byte)Keys.A == b)
                player.Rotation -= 0.05f;
            if ((byte)Keys.S == b)
            {
                player.X = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation + (float)Math.PI, player.Speed / 5 * 2).X;
                player.Y = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation + (float)Math.PI, player.Speed / 5 * 2).Y;
            }
        }
    }
}
