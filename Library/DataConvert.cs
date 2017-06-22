using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Messenger;
using Library.Tiles;
using Lidgren.Network;

namespace Library
{
    public static class DataConvert
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";

        public static ITile ReadTile(NetIncomingMessage inc)
        {
            var tileType = (TileType) inc.ReadByte();
            var typeString = string.Concat("Library.Tiles.", tileType.ToString("G"));
            var tile = (ITile)GetInstance(typeString);

            tile.Read(inc);

            return tile;
        }

        public static object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }

        public static void WritePlayer(NetOutgoingMessage outmsg, Player player, int inputId = -1)
        {
            outmsg.Write(player.Username);
            outmsg.Write(player.X);
            outmsg.Write(player.Y);
            outmsg.Write(inputId);
        }

        public static int ReadPlayer(NetIncomingMessage inc, Player player)
        {
            player.Username = inc.ReadString();
            player.X = inc.ReadFloat();
            player.Y = inc.ReadFloat();
            return inc.ReadInt32();
        }

        public static void WriteMessage(NetOutgoingMessage outmsg, Message message)
        {
            outmsg.Write(message.Sender);
            outmsg.Write(message.Text);
            outmsg.Write(message.Timestamp.ToString(DateTimeFormat));
        }

        public static void ReadMessage(NetIncomingMessage inc, Message message)
        {
            message.Sender = inc.ReadString();
            message.Text = inc.ReadString();
            message.Timestamp = DateTime.ParseExact(inc.ReadString(), DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
        }

        public static void ReadRoom(NetIncomingMessage inc, GameRoom room)
        {
            room.Name = inc.ReadString();
            room.Map = ReadMap(inc);
            ReadPlayerList(inc, room.Players);
        }

        public static void WriteRoom(NetOutgoingMessage outmsg, GameRoom room)
        {
            outmsg.Write(room.Name);
            WriteMap(outmsg, room.Map);
            WritePlayerList(outmsg, room.Players);
        }

        public static void WriteMap(NetOutgoingMessage outmsg, Map map)
        {
            outmsg.Write((byte)map.MapSize);
            var rows = map.MapData.Count;
            outmsg.Write(rows);
            for (int i = 0; i < rows; i++)
            {
                var columnLength = map.MapData[i].Count;
                outmsg.Write(columnLength);
                for (int j = 0; j < columnLength; j++)
                {
                    map.MapData[i][j].Write(outmsg);
                }
            }
        }

        public static Map ReadMap(NetIncomingMessage inc)
        {
            var size = (MapSize)inc.ReadByte();
            var map = Map.GetEmptyMap(size);
            var rows = inc.ReadInt32();

            for (int i = 0; i < rows; i++)
            {
                var columnLength = inc.ReadInt32();
                map.MapData.Add(new List<ITile>());

                for (int j = 0; j < columnLength; j++)
                {
                    map.MapData[i][j] = ReadTile(inc);
                }
            }
            return map;
        }

        public static void WritePlayerList(NetOutgoingMessage outmsg, List<Player> players)
        {
            var playerCount = players.Count;
            outmsg.Write(playerCount);
            for (int i = 0; i < playerCount; i++)
            {
                WritePlayer(outmsg, players[i]);
            }
        }

        public static void ReadPlayerList(NetIncomingMessage inc, List<Player> players)
        {
            var playerCount = inc.ReadInt32();
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new Player());
                ReadPlayer(inc, players[i]);
            }
        }
    }
}
