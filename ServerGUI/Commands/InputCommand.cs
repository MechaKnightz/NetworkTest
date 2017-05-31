using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> allRooms)
        {

            var inputId = -1;
            var dirty = false;
            var dirtyPlayer = new Player();

            foreach (var player2 in allPlayers)
            {
                if (player2.Conn != inc.SenderConnection)
                    continue;

                inputId = inc.ReadInt32();

                var key = (Keys)inc.ReadByte();

                var tempRoom = new GameRoom();
                foreach (var gameRoom in allRooms)
                {
                    if (gameRoom.Players.Any(x => x.Username == player2.Username)) tempRoom = gameRoom;
                }

                ReadInput(player2, tempRoom, key);

                dirty = true;
                dirtyPlayer = player2;
            }
            if (dirty)
            {
                var command = new SendPlayerCommand(inputId);
                command.Run(loggerManager, null, server, inc, dirtyPlayer, allPlayers, allRooms);
                return;
            }
            loggerManager.ServerMsg("Couldn't find player with " + inc.SenderConnection);
        }

        private void ReadInput(Player player, GameRoom gameRoom, Keys key)
        {
            var tempPlayer = (Player)player.Clone();

            InputHandler.MovePlayer(tempPlayer, key);

            if(CollisionManager.CheckCollision(tempPlayer, gameRoom.Players))
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
                    //TODO jump command
                    //var command = new ShootCommand();
                    //command.Run(LoggerManager, null, Server, Inc, player, allPlayers, allRooms);
                    break;
            }
        }
    }
}
