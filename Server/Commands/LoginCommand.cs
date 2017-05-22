using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Server.Commands
{
    class LoginCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            Console.WriteLine("Incoming login");

            var name = inc.ReadString();

            if (world.Players.Any(x => x.Username == name))
            {
                var deniedReason = "Denied connection, duplicate client.";
                inc.SenderConnection.Deny(deniedReason);
                Console.Write(deniedReason);
                return;
            }
            inc.SenderConnection.Approve();
            Console.WriteLine("Approved client connection");

            CreatePlayer(inc, name, world);

            NetOutgoingMessage outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.StartState);

            outmsg.Write(world.Circles.Count);
            foreach (var circle in world.Circles)
            {
                NetReader.WriteCircle(outmsg, circle);
            }
            outmsg.Write(world.Players.Count);
            foreach (var worldPlayer in world.Players)
            {
                NetReader.WritePlayer(outmsg, worldPlayer);
            }

            //connectionmessage:
            //packet
            //circle count
            //all circle info
            //player count
            //all player info

            System.Threading.Thread.Sleep(500);

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            Console.WriteLine("Approved new connection and updated the world status");
        }

        private static void CreatePlayer(NetIncomingMessage inc, string name, World world)
        {

            var intersects = false;
            for (int i = 0; i < int.MaxValue; i++)
            {
                intersects = false;
                var newPlayer = new Player(name, new Vector2(i * 200, 0), 10f, 0f, 5f, 50, 5, inc.SenderConnection);
                var circle = new Circle(newPlayer.Radius, newPlayer.X, newPlayer.Y);
                foreach (var worldPlayer in world.Players)
                {
                    intersects = false;
                    var tempCircle = new Circle(worldPlayer.Radius, worldPlayer.X, worldPlayer.Y);
                    if (circle.Intersect(tempCircle))
                    {
                        intersects = true;
                    }
                }
                if (intersects)
                {
                    Console.WriteLine("spawnpoint obstructed, moving player to position: " + new Vector2((i + 1) * 200, 0));
                    continue;
                }
                world.Players.Add(newPlayer);
                break;
            }
        }
    }
}
