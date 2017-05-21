using System;
using System.Threading;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using ServerGUI.Commands;
using ServerGUI.ServerLogger;

namespace ServerGUI
{
    public class Server
    {
        public World World { get; set; }
        public NetServer NetServer { get; private set; }
        public LoggerManager LoggerManager;

        public Timer UpdateTimer;

        public Server(LoggerManager loggerManager, World world)
        {
            World = world;
            LoggerManager = loggerManager;

            NetPeerConfiguration config = new NetPeerConfiguration("testGame");
            config.MaximumConnections = 32;
            config.Port = 9911;

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetServer = new NetServer(config);
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
                        if (inc.ReadByte() == (byte)PacketTypes.Login)
                        {
                            var login = new LoginCommand();
                            login.Run(LoggerManager, NetServer, inc, null, World);
                            continue;
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

        public void Data(NetIncomingMessage inc)
        {
            var command = CommandHandler.GetCommand(inc);
            command.Run(LoggerManager, NetServer, inc, null, World);
        }

        private void StatusChanged(NetIncomingMessage inc)
        {
            LoggerManager.ServerMsg(inc.SenderConnection + " status changed: " + inc.SenderConnection.Status);
            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
            {
                foreach (var player in World.Players)
                {
                    if (player.Conn == inc.SenderConnection)
                    {
                        World.Players.Remove(player);
                        LoggerManager.ServerMsg("Removed player " + player.Username);
                        break;
                    }
                }
            }
        }

        private void Update()
        {
            LoggerManager.ServerMsg("test");

        }
    }
}
