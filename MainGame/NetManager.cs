using System;
using System.Linq;
using Library;
using Library.Messenger;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class NetManager
    {
        private NetClient Client { get; set; }
        public World World { get; set; }
        public World LocalWorld { get; set; }
        public string Username { get; set; }

        private const float interpolationConst = 0.3f;

        public bool Initialize(string name, string hostip, int port)
        {
            World = new World();
            LocalWorld = new World();
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
                                LocalWorld.Circles.Add(circle);
                            }

                            var count2 = inc.ReadInt32();

                            for (int i = 0; i < count2; i++)
                            {
                                var player = new Player();

                                NetReader.ReadPlayer(inc, player);

                                World.Players.Add(player);
                                LocalWorld.Players.Add(player);
                            }

                            var count3 = inc.ReadInt32();

                            for (int i = 0; i < count3; i++)
                            {
                                var message = new Message();

                                NetReader.ReadMessage(inc, message);

                                World.ChatMessages.Add(message);
                                LocalWorld.ChatMessages.Add(message);
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
        public void CheckServerMessages(GameTime gameTime)
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
                    Data(inc, gameTime);
                }
            }
        }

        private void Data(NetIncomingMessage inc, GameTime gameTime)
        {
            var incomingPacket = (PacketTypes)inc.ReadByte();
            switch (incomingPacket)
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
                case PacketTypes.AllShots:
                    var count3 = inc.ReadInt32();
                    World.Shots.Clear();
                    for (int i = 0; i < count3; i++)
                    {
                        Shot shot = new Shot();
                        NetReader.ReadShot(inc, shot);
                        World.Shots.Add(shot);
                        
                        if(LocalWorld.Shots.Count <= i)
                            LocalWorld.Shots.Add(new Shot(shot.X, shot.Y, shot.Rotation, shot.Speed, shot.Damage, shot.Radius, shot.Duration, shot.ParentName));
                        

                        var tempLoc = Interpolate(new Vector2(LocalWorld.Shots[i].X, LocalWorld.Shots[i].Y),
                            new Vector2(shot.X, shot.Y),
                            gameTime.ElapsedGameTime.Milliseconds);
                        Shot shot2 = new Shot(tempLoc.X, tempLoc.Y, shot.Rotation, shot.Speed, shot.Damage, shot.Radius, shot.Duration, shot.ParentName);

                        LocalWorld.Shots[i] = shot2;
                    }

                    for (int i = count3; i < LocalWorld.Shots.Count; i++)
                    {
                        LocalWorld.Shots.RemoveAt(i);
                    }
                    break;
            }
        }

        private Vector2 Interpolate(Vector2 local, Vector2 remote, float deltaTime = 100 / 60f)
        {
            var difference = remote.X - local.X;
            if (difference < 1000)
                local.X = remote.X;
            else
                local.X += difference * deltaTime * interpolationConst;

            var difference2 = remote.Y - local.Y;
            if (difference2 < 1000)
                local.Y = remote.Y;
            else
                local.Y += difference * deltaTime * interpolationConst;

            return local;
        }
    }
}
