using System;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ServerGUI.Commands;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    class InputCommand : ICommand
    {
        private LoggerManager LoggerManager;
        private World World;
        private NetServer Server;
        private NetIncomingMessage Inc;

        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            LoggerManager = loggerManager;
            Server = server;
            Inc = inc;
            World = world;

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
            loggerManager.ServerMsg("Couldn't find player with " + inc.SenderConnection);
        }

        private void ReadInput(Player player, World world, Keys key)
        {
            var tempPlayer = (Player)player.Clone();

            MovePlayer(tempPlayer, key);

            if(CollisionManager.CheckCollision(tempPlayer, world))
            {
                return;
            }

            if (CollisionManager.CheckCollisionCircles(tempPlayer, World))
            {
                return;
            }

            HandleKey(player, key);
        }

        private void HandleKey(Player player, Keys key)
        {
            switch (key)
            {
                case Keys.Space:
                    PlayerShoot(player, key);
                    break;
                case Keys.W:
                    MovePlayer(player, key);
                    break;
                case Keys.A:
                    MovePlayer(player, key);
                    break;
                case Keys.S:
                    MovePlayer(player, key);
                    break;
                case Keys.D:
                    MovePlayer(player, key);
                    break;
            }
        }

        private void PlayerShoot(Player player, Keys key)
        {
            switch (key)
            {
                case Keys.Space:
                    var command = new ShootCommand();
                    command.Run(LoggerManager, Server, Inc, player, World);
                    break;
            }
        }

        private void MovePlayer(Player player, Keys key)
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
