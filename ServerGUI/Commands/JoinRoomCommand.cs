using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class JoinRoomCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player,
            List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            var roomName = inc.ReadString();
            var tempPlayer = GetPlayerFromConnection(inc, allPlayers);

            var gameRoom = GetGameRoom(roomName, gameRooms);
            gameRoom.Players.Add(tempPlayer);

            gameRooms.Add(gameRoom);
            
            var command = new SendRoomStartState();
            command.Run(loggerManager, mongoClient, server, inc, player, allPlayers, gameRooms);

            var outmsg = server.CreateMessage();
            
            outmsg.Write((byte)PacketTypes.PlayerPosition);
            NetReader.WritePlayer(outmsg, tempPlayer);
            
            for (int i = 0; i < gameRoom.Players.Count; i++)
            {
                server.SendMessage(outmsg, gameRoom.Players[i].Conn, NetDeliveryMethod.ReliableOrdered);
            }
            loggerManager.ServerMsg(tempPlayer + " joined room " + gameRoom.Name);
        }

        private GameRoom GetGameRoom(string name, List<GameRoom> gameRooms)
        {
            var tempRoom = gameRooms.FirstOrDefault(x => x.Name == name);

            if (tempRoom == null)
            {
                tempRoom = new GameRoom(name);
            }
            return tempRoom;
        }

        private Player GetPlayerFromConnection(NetIncomingMessage inc, List<Player> allPlayers)
        {
            return allPlayers.FirstOrDefault(x => x.Conn == inc.SenderConnection);
        }
    }
}
