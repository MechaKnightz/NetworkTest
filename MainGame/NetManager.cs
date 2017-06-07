using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using Library.Tiles;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MapMaker.Tiles;

namespace MainGame
{
    public class NetManager
    {
        private NetClient Client { get; set; }
        public List<GameRoom> GameRooms { get; set; }
        public GameRoom CurrentRoom { get; set; }
        public string Username { get; set; }
        public List<Keys> Input { get; set; }

        public bool Initialize(string name, string password, string hostip, int port, out string msg)
        {
            Input = new List<Keys>();
            GameRooms = new List<GameRoom>();
            CurrentRoom = new GameRoom();
            Username = name;
            NetPeerConfiguration config = new NetPeerConfiguration("testGame");
            Client = new NetClient(config);

            Client.Start();

            NetOutgoingMessage outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.Login);

            outmsg.Write(name);
            outmsg.Write(password);

            Client.Connect(hostip, port, outmsg);
            
            var tempBool = WaitForStartingInfo(Client, out msg);

            return tempBool;
        }

        private bool WaitForStartingInfo(NetClient client, out string msg)
        {
            var time = DateTime.Now;

            NetIncomingMessage inc;

            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 10)
                {
                    msg = "Couldn't find server";
                    return false;
                }
                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        if (inc.ReadByte() == (byte) PacketTypes.StartState)
                        {
                            var count1 = inc.ReadInt32();

                            GameRooms.Clear();
                            for (int i = 0; i < count1; i++)
                            {
                                var tempRoom = new GameRoom();
                                tempRoom.Name = inc.ReadString();
                                GameRooms.Add(tempRoom);
                            }

                            msg = "Successfully connected to lobby";
                            return true;
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)inc.ReadByte())
                        {
                            //When connected to the server
                            case NetConnectionStatus.Connected:
                                break;
                            //When disconnected from the server
                            case NetConnectionStatus.Disconnected:
                                {
                                    string reason = inc.ReadString();
                                    if (string.IsNullOrEmpty(reason))
                                    {
                                        msg = "Connection denied";
                                        return false;
                                    }
                                    msg = "Connection denied, reason: " + reason;
                                    return false;
                                }
                                break;
                        }
                        break;
                }
            }
        }

        public void SendInput(Keys key)
        {
            var outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.KeyInput);

            Input.Add(key);
            outmsg.Write(Input.Count - 1);

            var localPlayer = CurrentRoom.Players.FirstOrDefault(x => x.Username == Username);

            if (localPlayer != null)
            {
                CurrentRoom.HandleInput(localPlayer, key);
            }

            outmsg.Write((byte)key);

            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendMouseInput(MouseButton button, float x, float y)
        {
            var outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.MouseInput);

            outmsg.Write((byte)button);

            outmsg.Write(x);

            outmsg.Write(y);

            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void CheckServerMessages(GameTime gameTime)
        {
            NetIncomingMessage inc;

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

                    var inputId = Library.DataConvert.ReadPlayer(inc, incPlayer);

                    var oldPlayer = CurrentRoom.Players.FirstOrDefault(x => x.Username == incPlayer.Username);

                    if (oldPlayer != null)
                    {
                        oldPlayer.X = incPlayer.X;
                        oldPlayer.Y = incPlayer.Y;

                        if (inputId != -1)
                        {
                            InputPrediction(oldPlayer, inputId);
                        }
                    }
                    else
                    {
                        CurrentRoom.Players.Add(incPlayer);
                    }
                    
                    break;
                case PacketTypes.AllPlayerPosition:
                    CurrentRoom.Players.Clear();
                    var count = inc.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        Player player2 = new Player();
                        Library.DataConvert.ReadPlayer(inc, player2);
                        CurrentRoom.Players.Add(player2);
                    }
                    break;
                case PacketTypes.PlayerHealth:

                    var playerName = inc.ReadString();

                    var playerHealth = inc.ReadFloat();

                    var player = CurrentRoom.Players.FirstOrDefault(x => x.Username == playerName);

                    if(player != null) player.Health = playerHealth;
                    break;
                case PacketTypes.PlayerLeave:
                    var name = inc.ReadString();

                    for (int i = 0; i < CurrentRoom.Players.Count; i++)
                    {
                        if(CurrentRoom.Players[i].Username == name) CurrentRoom.Players.RemoveAt(i);
                    }
                    break;
                case PacketTypes.TileData:
                    var row = inc.ReadInt32();
                    var column = inc.ReadInt32();
                    CurrentRoom.Map.MapData[row][column] = DataConvert.ReadTile(inc);
                    break;
            }
        }

        private void InputPrediction(Player player, int inputId)
        {
            for (int i = 0; i < Input.Count; i++)
            {
                if(inputId > i) continue;

                InputHandler.MovePlayer(player, CurrentRoom.Map, Input[i]);
            }
        }

        public bool Register(string name, string password, string hostip, int port, out string msg)
        {
            Username = name;
            NetPeerConfiguration config = new NetPeerConfiguration("testGame");
            Client = new NetClient(config);

            Client.Start();

            NetOutgoingMessage outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.Register);

            outmsg.Write(name);
            outmsg.Write(password);

            Client.Connect(hostip, port, outmsg);

            return WaitForRegisterInfo(Client, out msg);
        }

        private bool WaitForRegisterInfo(NetClient client, out string msg)
        {
            var time = DateTime.Now;

            NetIncomingMessage inc;

            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 10)
                {
                    msg = "Couldn't find server";
                    return true;
                }
                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)inc.ReadByte())
                        {
                            //When connected to the server
                            case NetConnectionStatus.Connected:
                                break;
                            //When disconnected from the server
                            case NetConnectionStatus.Disconnected:
                                {
                                    string reason = inc.ReadString();
                                    if (string.IsNullOrEmpty(reason))
                                    {
                                        msg = "Couldn't resolve register message.";
                                        return true;
                                    }
                                    msg = reason;
                                    return true;
                                }
                        }
                        break;
                }
            }
        }

        public bool SendJoinRoomInput(string roomName, out string msg)
        {
            var outmsg = Client.CreateMessage();

            outmsg.Write((byte)PacketTypes.JoinRoom);

            outmsg.Write(roomName);

            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

            return WaitForRoomInfo(Client, out msg);
        }

        private bool WaitForRoomInfo(NetClient client, out string msg)
        {
            var time = DateTime.Now;

            NetIncomingMessage inc;

            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 5)
                {
                    msg = "Couldn't connect to room";
                    return false;
                }
                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        if (inc.ReadByte() == (byte)PacketTypes.RoomStartState)
                        {
                            CurrentRoom = new GameRoom();
                            Library.DataConvert.ReadRoom(inc, CurrentRoom);
                            msg = "Successfully connected to room";
                            return true;
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)inc.ReadByte())
                        {
                            //When connected to the server
                            case NetConnectionStatus.Connected:
                                break;
                            //When disconnected from the server
                            case NetConnectionStatus.Disconnected:
                            {
                                string reason = inc.ReadString();
                                if (string.IsNullOrEmpty(reason))
                                {
                                    msg = "Connection denied";
                                    return false;
                                }
                                msg = "Connection denied, reason: " + reason;
                                return false;
                            }
                        }
                        break;
                }
            }
        }
    }
}
