using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    class LoginCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            loggerManager.ServerMsg("Incoming login");

            var name = inc.ReadString();

            if (world.Players.Any(x => x.Username == name))
            {
                var deniedReason = "Denied connection, duplicate client.";
                inc.SenderConnection.Deny(deniedReason);
                loggerManager.ServerMsg(deniedReason);
                return;
            }
            inc.SenderConnection.Approve();
            loggerManager.ServerMsg("Approved client connection");

            CreatePlayer(loggerManager, inc, name, world);

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
            outmsg.Write(world.ChatMessages.Count);
            foreach (var message in world.ChatMessages)
            {
                NetReader.WriteMessage(outmsg, message);
            }

            //connectionmessage:
            //packet
            //circle count
            //all circle info
            //player count
            //all player info

            System.Threading.Thread.Sleep(500);

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            loggerManager.ServerMsg("Approved new connection and updated the world status");
        }

        private static void CreatePlayer(LoggerManager loggerManager, NetIncomingMessage inc, string name, World world)
        {

            var intersects = false;
            for (int i = 0; i < int.MaxValue; i++)
            {
                intersects = false;
                var newPlayer = new Player(name, new Vector2(i * 200, 0), 10f, 0f, 5f, 50, inc.SenderConnection);
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
                    loggerManager.ServerMsg("spawnpoint obstructed, moving player to position: " + new Vector2((i + 1) * 200, 0));
                    continue;
                }
                world.Players.Add(newPlayer);
                break;
            }
        }
    }
}
