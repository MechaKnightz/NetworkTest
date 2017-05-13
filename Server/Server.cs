using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Lidgren.Network;
using Lidgren;
using Microsoft.Xna.Framework.Input;

namespace Server
{
    public static class Server
    {
        private static World _world = new World();

        public static void Start()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter map file name: ");

                    string mapName = StringCheck.MakeValidFileName(Console.ReadLine());

                    string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mapName + ".json");

                    var saveString = File.ReadAllText(destPath);

                    _world.Circles = JsonConvert.DeserializeObject<List<Circle>>(saveString);

                }
                catch(Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("Could not load map file.");
                    continue;
                }
                Console.WriteLine("Successfully imported map.");
                break;
            }

            NetPeerConfiguration config = new NetPeerConfiguration("testGame");
            config.MaximumConnections = 32;
            config.Port = 9911;

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetServer server = new NetServer(config);
            
            server.Start();

            Console.WriteLine("Server started at IP: " + "Unknown" + " and port: " + server.Port);

            NetIncomingMessage inc;

            DateTime time = DateTime.Now;

            TimeSpan timetopass = new TimeSpan(0, 0, 0, 0, 30);

            Console.WriteLine("Waiting for new connections and updating world state to current ones");

            while (true)
            {
                if ((inc = server.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(inc.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionApproval:

                        if (inc.ReadByte() == (byte) PacketTypes.Login)
                        {
                            Console.WriteLine("Incoming login");

                            var name = inc.ReadString();

                            if (_world.Players.Any(x => x.Name == name))
                            {
                                var deniedReason = "Denied connection, duplicate client.";
                                inc.SenderConnection.Deny(deniedReason);
                                Console.Write(deniedReason);
                                continue;
                            }
                            inc.SenderConnection.Approve();
                            Console.WriteLine("Approved client connection");

                            _world.Players.Add(new Player(name, new Vector2(0, 0), inc.SenderConnection));

                            NetOutgoingMessage outmsg = server.CreateMessage();

                            outmsg.Write((byte)PacketTypes.StartState);

                            outmsg.Write(_world.Circles.Count);
                            foreach (var circle in _world.Circles)
                            {
                                WriteCircle(outmsg, circle);
                            }
                            outmsg.Write(_world.Players.Count);
                            foreach (var player in _world.Players)
                            {
                                WritePlayer(outmsg, player);
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
                        break;
                    case NetIncomingMessageType.Data:
                        if (inc.ReadByte() == (byte) PacketTypes.Move)
                        {
                            foreach (var player in _world.Players)
                            {
                                if (player.Conn != inc.SenderConnection)
                                    continue;

                                var b = inc.ReadByte();

                                if ((byte)Keys.D == b)
                                    player.X++;
                                if ((byte)Keys.W == b)
                                    player.Y--;
                                if ((byte)Keys.A == b)
                                    player.X--;
                                if ((byte)Keys.S == b)
                                    player.Y++;

                                NetOutgoingMessage outmsg = server.CreateMessage();

                                outmsg.Write((byte)PacketTypes.WorldState);

                                outmsg.Write(_world.Players.Count);

                                foreach (var player2 in _world.Players)
                                {
                                    WritePlayer(outmsg, player2);
                                }

                                //connectionmessage:
                                //packet
                                //player count
                                //all player info

                                server.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
                            }
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:

                        Console.WriteLine(inc.SenderConnection + " status changed. " + inc.SenderConnection.Status);
                        if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            foreach (var player in _world.Players)
                            {
                                if (player.Conn == inc.SenderConnection)
                                {
                                    _world.Players.Remove(player);
                                    break;
                                }
                            }
                        }
                        break;
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private static void WriteCircle(NetOutgoingMessage outmsg, Circle circle)
        {
            outmsg.Write(circle.Radius);
            outmsg.Write(circle.X);
            outmsg.Write(circle.Y);
        }

        private static void WritePlayer(NetOutgoingMessage outmsg, Player player)
        {
            outmsg.Write(player.Name);
            outmsg.Write(player.X);
            outmsg.Write(player.Y);
        }
    }
}
