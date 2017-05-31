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
    public class SendPlayerCommand : ICommand
    {
        private readonly int _inputId = -1;
        public SendPlayerCommand() { }

        public SendPlayerCommand(int inputId)
        {
            _inputId = inputId;
        }
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {

            if (_inputId == -1)
            {
                NetOutgoingMessage outmsg = server.CreateMessage();

                outmsg.Write((byte)PacketTypes.PlayerPosition);

                NetReader.WritePlayer(outmsg, player);

                server.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
            }

            NetOutgoingMessage outmsg2 = server.CreateMessage();

            outmsg2.Write((byte)PacketTypes.PlayerPosition);

            NetReader.WritePlayer(outmsg2, player, _inputId);

            var tempRoom = new GameRoom();
            foreach (var room in gameRooms)
            {
                if (room.Players.Any(x => x.Username == player.Username)) tempRoom = room;
            }

            foreach (var tempPlayer in tempRoom.Players)
            {
                server.SendMessage(outmsg2, tempPlayer.Conn, NetDeliveryMethod.ReliableOrdered);
            }
        }
    }
}
