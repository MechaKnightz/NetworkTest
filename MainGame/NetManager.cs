using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class NetManager
    {
        private NetClient Client { get; set; }
        public World World { get; } = new World();
        public string Username { get; set; }

        public bool Initialize(string name, string hostip, int port)
        {
            Username = name;
            NetPeerConfiguration config = new NetPeerConfiguration("testGame");
            Client = new NetClient(config);

            Client.Start();

            NetOutgoingMessage outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.Login);

            outmsg.Write(name);

            Client.Connect(hostip, port, outmsg);

            return WaitForStartingInfo(Client);
        }

        private bool WaitForStartingInfo(NetClient client)
        {
            var time = DateTime.Now;

            NetIncomingMessage inc;

            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 5)
                {
                    return false;
                }
                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        if (inc.ReadByte() == (byte) PacketTypes.StartState)
                        {
                            var count1 = inc.ReadInt32();

                            for (int i = 0; i < count1; i++)
                            {
                                var circle =  new Circle();

                                circle = NetReader.ReadCircle(inc, circle);

                                World.Circles.Add(circle);
                            }

                            var count2 = inc.ReadInt32();

                            for (int i = 0; i < count2; i++)
                            {
                                var player = new Player();

                                NetReader.ReadPlayer(inc, player);

                                World.Players.Add(player);
                            }
                            return true;
                        }
                        break;
                }
            }
        }

        public void SendInput(Keys key)
        {
            var outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.Move);

            outmsg.Write((byte)key);

            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
        public void CheckServerMessages()
        {
            // Create new incoming message holder
            NetIncomingMessage inc;

            // While theres new messages
            //
            // THIS is exactly the same as in WaitForStartingInfo() function
            // Check if its Data message
            // If its PlayerPos, read all the characters to list
            while ((inc = Client.ReadMessage()) != null)
            {
                if (inc.MessageType == NetIncomingMessageType.Data)
                {
                    Data(inc);
                }
            }
        }

        private void Data(NetIncomingMessage inc)
        {
            var incomingPacket = inc.ReadByte();
            switch ((PacketTypes) incomingPacket)
            {
                case PacketTypes.PlayerPosition:
                    Player incPlayer = new Player();
                    NetReader.ReadPlayer(inc, incPlayer);
                    var oldPlayer = World.Players.FirstOrDefault(x => x.Username == incPlayer.Username);

                    if (oldPlayer != null)
                    {
                        oldPlayer.X = incPlayer.X;
                        oldPlayer.Y = incPlayer.Y;
                        oldPlayer.Rotation = incPlayer.Rotation;
                    }
                    else
                    {
                        World.Players.Add(incPlayer);
                    }
                    
                    break;
                case PacketTypes.AllPlayerPosition:
                    World.Players.Clear();
                    var count = inc.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        Player player2 = new Player();
                        NetReader.ReadPlayer(inc, player2);
                        World.Players.Add(player2);
                    }
                    break;
            }
        }
    }
}
