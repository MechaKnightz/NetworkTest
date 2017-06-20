using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using MongoDB.Driver;
using ServerGUI.Commands;
using ServerGUI.ServerLogger;

namespace ServerGUI
{
    public class Server
    {
        public NetServer NetServer { get; }
        private LoggerManager LoggerManager;
        private MongoClient MongoClient { get; }

        public List<GameRoom> GameRooms { get; set; }
        public List<Player> AllPlayers { get; set; }

        public Timer UpdateTimer;

        public Server(LoggerManager loggerManager, List<Player> allPlayers, string mongoUsername, string mongoPass)
        {
            AllPlayers = allPlayers;
            LoggerManager = loggerManager;
            GameRooms = new List<GameRoom>();

            NetPeerConfiguration config = new NetPeerConfiguration("TerraStructor")
            {
                MaximumConnections = 32,
                Port = 9911
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetServer = new NetServer(config);

            MongoClient = new MongoClient("mongodb://" + mongoUsername + 
                ":" + mongoPass +
                "@cluster0-shard-00-00-kp9r9.mongodb.net:27017,cluster0-shard-00-01-kp9r9.mongodb.net:27017,cluster0-shard-00-02-kp9r9.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");

            loggerManager.ServerMsg("Logged into MongoDB database as user: " + mongoUsername);
        }

        public void Run()
        {
            LoggerManager.ServerMsg("Server started at local IP: '" + GetLocalIPAddress() + "', external IP: '"+ GetExternalIPAddress() + "' and port: '" + NetServer.Configuration.Port + "'");
            
            LoggerManager.ServerMsg("Waiting for new connections and updating world state to current ones");

            UpdateTimer = new Timer(_ => Update(), null, 0, 16 + 2 / 3);

            NetServer.Start();
            LoggerManager.ServerMsg("Server started...");

            while (true)
            {
                //TODO fix server loop stopping after 2 room joins
                NetIncomingMessage inc;
                if ((inc = NetServer.ReadMessage()) == null) continue;
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        var connectionType = (PacketTypes)inc.ReadByte();
                        switch (connectionType)
                        {
                            case PacketTypes.Login:
                            {
                                var login = new LoginCommand();
                                login.Run(LoggerManager, MongoClient, NetServer, inc, null, AllPlayers, GameRooms);
                                continue;
                            }
                            case PacketTypes.Register:
                            {
                                var login = new RegisterCommand();
                                login.Run(LoggerManager, MongoClient, NetServer, inc, null, AllPlayers, GameRooms);
                                continue;
                            }
                        }

                        var deniedReason = "Faulty connection type";
                        inc.SenderConnection.Deny(deniedReason);
                        LoggerManager.ServerMsg(deniedReason);
                        break;
                    case NetIncomingMessageType.Data:
                        Data(inc);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        StatusChanged(inc);
                        break;
                }
            }
        }

        private void Data(NetIncomingMessage inc)
        {
            var command = CommandHandler.GetCommand(inc);
            command.Run(LoggerManager, null, NetServer, inc, null, AllPlayers, GameRooms);
        }

        private void StatusChanged(NetIncomingMessage inc)
        {
            LoggerManager.ServerMsg(inc.SenderConnection + " status changed: " + inc.SenderConnection.Status);
            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
            {
                foreach (var player in AllPlayers)
                {
                    if (player.Conn != inc.SenderConnection) continue;

                    var command = new KickPlayerCommand();
                    command.Run(LoggerManager, MongoClient, NetServer, null, player, AllPlayers, GameRooms);
                    break;
                }
            }
        }

        private void Update()
        {
            GameRoom.GravityMovePlayers(GameRooms);

            SendPlayerIfDirty();

            SendTileIfDirty();
        }

        public static GameRoom GetGameRoom(Player player, List<GameRoom> gameRooms)
        {
            foreach (var room in gameRooms)
            {
                if (room.Players.Any(x => x.Username == player.Username))
                {
                    return room;
                }
            }
            return null;
        }

        public static Player GetPlayer(NetIncomingMessage inc, List<Player> allPlayers)
        {
            return allPlayers.FirstOrDefault(x => x.Conn == inc.SenderConnection);
        }

        public static void SendToGameRoomPlayers(NetServer server, NetOutgoingMessage outmsg, Player player,
            List<GameRoom> gameRooms)
        {
            var room = GetGameRoom(player, gameRooms);
            SendToGameRoomPlayers(server, outmsg, room);
        }

        public static void SendToGameRoomPlayers(NetServer server, NetOutgoingMessage outmsg, GameRoom room)
        {
            var recipients = new List<NetConnection>();
            for (int i = 0; i < room.Players.Count; i++)
            {
                recipients.Add(room.Players[i].Conn);
            }
            if(recipients.Count <= 0) return;
            server.SendMessage(outmsg, recipients, NetDeliveryMethod.ReliableOrdered, 0);
        }

        private void SendPlayerIfDirty()
        {
            for (int i = 0; i < GameRooms.Count; i++)
            {
                for (int j = 0; j < GameRooms[i].Players.Count; j++)
                {
                    if (GameRooms[i].Players[j].IsDirty)
                    {
                        for (int k = 0; k < GameRooms[i].Players.Count; k++)
                        {
                            var outmsg = NetServer.CreateMessage();
                            outmsg.Write((byte)PacketTypes.PlayerPosition);

                            if (GameRooms[i].Players[k].Username == GameRooms[i].Players[j].Username)
                            {
                                DataConvert.WritePlayer(outmsg, GameRooms[i].Players[j], GameRooms[i].Players[j].LatestInput);
                            }
                            else
                            {
                                DataConvert.WritePlayer(outmsg, GameRooms[i].Players[j]);
                            }

                            NetServer.SendMessage(outmsg, GameRooms[i].Players[k].Conn,
                                NetDeliveryMethod.ReliableOrdered);
                        }
                        GameRooms[i].Players[j].IsDirty = false;
                    }
                }
            }
        }

        private void SendTileIfDirty()
        {
            //TODO make a list of all the dirty tiles instead of making the server search through all of them
            for (int i = 0; i < GameRooms.Count; i++)
            {
                if(!GameRooms[i].Map.Dirty) continue;
                for (int j = 0; j < GameRooms[i].Map.MapData.Count; j++)
                {
                    for (int k = 0; k < GameRooms[i].Map.MapData[j].Count; k++)
                    {
                        if(!GameRooms[i].Map.MapData[j][k].Dirty) continue;

                        var outmsg = NetServer.CreateMessage();

                        outmsg.Write((byte)PacketTypes.TileData);

                        outmsg.Write(j);
                        outmsg.Write(k);

                        GameRooms[i].Map.MapData[j][k].Write(outmsg);
                        
                        SendToGameRoomPlayers(NetServer, outmsg, GameRooms[i]);

                        GameRooms[i].Map.MapData[j][k].Dirty = false;
                    }
                }
                GameRooms[i].Map.Dirty = false;
            }
        }

        //not mine
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP Address Not Found!";
        }

        public static string GetExternalIPAddress()
        {
            string ip = new WebClient().DownloadString("http://icanhazip.com");
            string replacement = Regex.Replace(ip, @"\n", "");
            return replacement;
        }
    }
}
