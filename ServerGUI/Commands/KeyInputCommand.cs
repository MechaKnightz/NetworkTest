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
    class KeyInputCommand : ICommand
    {

        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> allRooms)
        {

            foreach (var player2 in allPlayers)
            {
                if (player2.Conn != inc.SenderConnection)
                    continue;

                //TODO fix player input prediction
                player2.LatestInput = inc.ReadInt32();

                var key = (Keys)inc.ReadByte();

                var tempRoom = new GameRoom();
                foreach (var gameRoom in allRooms)
                {
                    if (gameRoom.Players.Any(x => x.Username == player2.Username)) tempRoom = gameRoom;
                }

                ReadInput(player2, tempRoom, key);
            }
        }

        private void ReadInput(Player player, GameRoom gameRoom, Keys key)
        {
            gameRoom.HandleInput(player, key);
        }
    }
}
