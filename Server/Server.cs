using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
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
        private static Timer _update;

        public static void Start()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter map file name: ");

                    string mapName = StringCheck.MakeValidFileName(Console.ReadLine());


                    string destPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MapMaker\Saves\",
                        mapName + ".json");

                    var saveString = File.ReadAllText(destPath);

                    _world.Circles = JsonConvert.DeserializeObject<List<Circle>>(saveString);

                }
                catch (Exception e)
                {
                    if(e is DirectoryNotFoundException)
                        Console.WriteLine("Invalid directoty: " + e.Message);
                    else if(e is FileNotFoundException)
                        Console.WriteLine("Invalid file name: " + e.Message);
                    else Console.WriteLine("Error: " + e.Message);

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
            _update =  new Timer(1.5625);
            _update.Elapsed += update_Elapsed;
            _update.Start();


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

                            CreatePlayer(inc, name);

                            NetOutgoingMessage outmsg = server.CreateMessage();

                            outmsg.Write((byte)PacketTypes.StartState);

                            outmsg.Write(_world.Circles.Count);
                            foreach (var circle in _world.Circles)
                            {
                                NetReader.WriteCircle(outmsg, circle);
                            }
                            outmsg.Write(_world.Players.Count);
                            foreach (var player in _world.Players)
                            {
                                NetReader.WritePlayer(outmsg, player);
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

                                ReadInput(player, b);

                                NetOutgoingMessage outmsg = server.CreateMessage();

                                outmsg.Write((byte)PacketTypes.WorldState);

                                outmsg.Write(_world.Players.Count);

                                foreach (var player2 in _world.Players)
                                {
                                    NetReader.WritePlayer(outmsg, player2);
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

                        Console.WriteLine(inc.SenderConnection + " status changed: " + inc.SenderConnection.Status);
                        if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            foreach (var player in _world.Players)
                            {
                                if (player.Conn == inc.SenderConnection)
                                {
                                    _world.Players.Remove(player);
                                    Console.WriteLine("Removed player " + player.Name);
                                    break;
                                }
                            }
                        }
                        break;
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private static void update_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }

        private static void ReadInput(Player player, byte b)
        {
            if ((byte)Keys.D == b)
                player.Rotation += 0.05f;
            if ((byte) Keys.W == b)
            {
                player.X = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation, player.Speed).X;
                player.Y = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation, player.Speed).Y;
            }
            if ((byte)Keys.A == b)
                player.Rotation -= 0.05f;
            if ((byte) Keys.S == b)
            {
                player.X = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation + (float)Math.PI, player.Speed / 5 * 2).X;
                player.Y = Angle.MoveAngle(new Vector2(player.X, player.Y), player.Rotation + (float)Math.PI, player.Speed / 5 * 2).Y;
            }
        }

        private static void CreatePlayer(NetIncomingMessage inc, string name)
        {
            _world.Players.Add(new Player(name, new Vector2(0, 0), 10f, 0f, 5f, inc.SenderConnection));

        }
    }
}
