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
using Server.Commands;

namespace Server
{
    public static class Server
    {
        private static World _world = new World();
        private static NetServer _server;

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

            _server = new NetServer(config);

            _server.Start();

            Console.WriteLine("Server started at IP: " + "Unknown" + " and port: " + _server.Port);

            NetIncomingMessage inc;

            DateTime time = DateTime.Now;

            TimeSpan timetopass = new TimeSpan(0, 0, 0, 0, 30);

            Console.WriteLine("Waiting for new connections and updating world state to current ones");

            while (true)
            {
                if ((inc = _server.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(inc.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        ReadData(inc);
                        break;
                    case NetIncomingMessageType.Data:
                        ReadData(inc);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        StatusChanged(inc);
                        break;
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private static void ReadData(NetIncomingMessage inc)
        {
            var command = CommandHandler.GetCommand(inc);
            command.Run(_server, inc, null, _world);

            //var packetType = inc.ReadByte();
            //switch ((PacketTypes)packetType)
            //{
            //    case PacketTypes.Move:
            //        Move(inc);
            //        break;
            //}
        }

        private static void StatusChanged(NetIncomingMessage inc)
        {
            Console.WriteLine(inc.SenderConnection + " status changed: " + inc.SenderConnection.Status);
            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
            {
                foreach (var player in _world.Players)
                {
                    if (player.Conn == inc.SenderConnection)
                    {
                        _world.Players.Remove(player);
                        Console.WriteLine("Removed player " + player.Username);
                        break;
                    }
                }
            }
        }
    }
}
