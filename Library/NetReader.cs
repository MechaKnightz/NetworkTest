using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Library
{
    public static class NetReader
    {

        public static void WriteCircle(NetOutgoingMessage outmsg, Circle circle)
        {
            outmsg.Write(circle.Radius);
            outmsg.Write(circle.X);
            outmsg.Write(circle.Y);
        }

        public static Circle ReadCircle(NetIncomingMessage inc, Circle circle)
        {
            circle.Radius = inc.ReadFloat();
            circle.X = inc.ReadFloat();
            circle.Y = inc.ReadFloat();

            return circle;
        }

        public static void WritePlayer(NetOutgoingMessage outmsg, Player player)
        {
            outmsg.Write(player.Name);
            outmsg.Write(player.X);
            outmsg.Write(player.Y);
            outmsg.Write(player.Health);
            outmsg.Write(player.Rotation);
        }

        public static void ReadPlayer(NetIncomingMessage inc, Player player)
        {
            player.Name = inc.ReadString();
            player.X = inc.ReadFloat();
            player.Y = inc.ReadFloat();
            player.Health = inc.ReadFloat();
            player.Rotation = inc.ReadFloat();
        }
    }
}
