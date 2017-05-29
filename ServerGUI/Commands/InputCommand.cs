using System;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MongoDB.Driver;
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

        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            LoggerManager = loggerManager;
            Server = server;
            Inc = inc;
            World = world;

            var inputId = -1;
            var dirty = false;
            var dirtyPlayer = new Player();

            foreach (var player2 in world.Players)
            {
                if (player2.Conn != inc.SenderConnection)
                    continue;

                inputId = inc.ReadInt32();

                var key = (Keys)inc.ReadByte();

                ReadInput(player2, world, key);

                dirty = true;
                dirtyPlayer = player2;
            }
            if (dirty)
            {
                var command = new SendPlayerCommand(inputId);
                command.Run(loggerManager, null, server, inc, dirtyPlayer, world);
                return;
            }
            loggerManager.ServerMsg("Couldn't find player with " + inc.SenderConnection);
        }

        private void ReadInput(Player player, World world, Keys key)
        {
            var tempPlayer = (Player)player.Clone();

            InputHandler.MovePlayer(tempPlayer, key);

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
                    InputHandler.MovePlayer(player, key);
                    break;
                case Keys.A:
                    InputHandler.MovePlayer(player, key);
                    break;
                case Keys.S:
                    InputHandler.MovePlayer(player, key);
                    break;
                case Keys.D:
                    InputHandler.MovePlayer(player, key);
                    break;
            }
        }

        private void PlayerShoot(Player player, Keys key)
        {
            switch (key)
            {
                case Keys.Space:
                    var command = new ShootCommand();
                    command.Run(LoggerManager, null, Server, Inc, player, World);
                    break;
            }
        }
    }
}
