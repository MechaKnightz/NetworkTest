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
    class SendAllPlayersCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            NetOutgoingMessage outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.AllPlayerPosition);

            outmsg.Write(allPlayers.Count);

            foreach (var worldPlayer in allPlayers)
            {
                Library.DataConvert.WritePlayer(outmsg, worldPlayer);
            }

            //connectionmessage:
            //packet
            //player count
            //all player info

            server.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
