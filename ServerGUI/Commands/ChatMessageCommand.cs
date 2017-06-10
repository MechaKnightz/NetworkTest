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

            if (sender.LastMessageTime + TimeSpan.FromSeconds(3) > DateTime.Now)
            {
                var spammsg = server.CreateMessage();

                spammsg.Write((byte)PacketTypes.ChatMessage);
                DataConvert.WriteMessage(spammsg, new Message("You are sending messages too fast", "[SERVER]"));
                server.SendMessage(spammsg, sender.Conn, NetDeliveryMethod.ReliableOrdered);
                return;
            }

            sender.LastMessageTime = DateTime.Now;

            var message = inc.ReadString();

            var fullMessage = new Message(message, sender.Username);

            var outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.ChatMessage);

            DataConvert.WriteMessage(outmsg, fullMessage);

            Server.SendToGameRoomPlayers(server, outmsg, sender, gameRooms);
        }
    }
}
