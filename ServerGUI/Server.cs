using System;
using System.Collections.Generic;
using System.Linq;
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
        public LoggerManager LoggerManager;
        public MongoClient MongoClient { get; set; }

        public List<GameRoom> GameRooms { get; set; }
        public List<Player> AllPlayers { get; set; }

        public Timer UpdateTimer;

        public Server(LoggerManager loggerManager, List<Player> allPlayers, string mongoUsername, string mongoPass)
        {
            AllPlayers = allPlayers;
            LoggerManager = loggerManager;
            GameRooms = new List<GameRoom>();

            NetPeerConfiguration config = new NetPeerConfiguration("testGame")
            {
                MaximumConnections = 32,
                Port = 9911
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetServer = new NetServer(config);

            MongoClient = new MongoClient("mongodb://" + mongoUsername + 
                ":" + mongoPass +
                "@cluster0-shard-00-00-kp9r9.mongodb.net:27017,cluster0-shard-00-01-kp9r9.mongodb.net:27017,cluster0-shard-00-02-kp9r9.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin");

        }

        public void Run()
        {
            LoggerManager.ServerMsg("Server started at IP: " + "Unknown" + " and port: " + NetServer.Port);
            
            LoggerManager.ServerMsg("Waiting for new connections and updating world state to current ones");

            UpdateTimer = new Timer(_ => Update(), null, 0, 16 + 2 / 3);

            NetServer.Start();
            LoggerManager.ServerMsg("Server started...");

            while (true)
            {
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
            // ReSharper disable once FunctionNeverReturns
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
                    AllPlayers.Remove(player);
                    LoggerManager.ServerMsg("Removed player " + player.Username);
                    break;
                }
            }
        }

        private void Update()
        {
            PlayerTileIntersection();

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
                            NetReader.WritePlayer(outmsg, GameRooms[i].Players[j]);

                            NetServer.SendMessage(outmsg, GameRooms[i].Players[k].Conn,
                                NetDeliveryMethod.ReliableOrdered);
                        }
                        GameRooms[i].Players[j].IsDirty = false;
                    }
                }
            }
        }

        private void PlayerTileIntersection()
        {
            //TODO Refactor into GameRoom class
            for (int i = 0; i < GameRooms.Count; i++)
            {
                for (int j = 0; j < GameRooms[i].Players.Count; j++)
                {
                    GameRooms[i].Intersection(GameRooms[i].Players[j]);
                }
            }

            for (int i = 0; i < GameRooms.Count; i++)
            {
                for (int j = 0; j < GameRooms[i].Players.Count; j++)
                {
                    if(!GameRooms[i].Players[j].Falling) continue;
                    GameRooms[i].Players[j].Y += GlobalConsts.GravityConst;
                }
            }
        }

        private static GameRoom GetGameRoom(Player player, List<GameRoom> gameRooms)
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
    }
}
