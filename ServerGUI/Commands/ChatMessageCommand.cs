using System;
using System.Collections.Generic;
using Library;
using Library.Messenger;
using Lidgren.Network;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class ChatMessageCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            var sender = Server.GetPlayer(inc, allPlayers);

            var message = inc.ReadString();

            var fullMessage = new Message(message, sender.Username);

            var outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.ChatMessage);

            DataConvert.WriteMessage(outmsg, fullMessage);

            Server.SendToGameRoomPlayers(server, outmsg, sender, gameRooms);
        }
    }
}
