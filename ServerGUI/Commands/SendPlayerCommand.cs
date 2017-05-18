using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class SendPlayerCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            NetOutgoingMessage outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.PlayerPosition);

            NetReader.WritePlayer(outmsg, player);

            //connectionmessage:
            //packet
            //player count
            //all player info

            server.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
