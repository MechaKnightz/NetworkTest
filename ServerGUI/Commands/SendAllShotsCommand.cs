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
    public class SendAllShotsCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {

            NetOutgoingMessage outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.AllShots);

            outmsg.Write(world.Shots.Count);

            for (int i = 0; i < world.Shots.Count; i++)
            {
                NetReader.WriteShot(outmsg, world.Shots[i]);
            }

            server.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
