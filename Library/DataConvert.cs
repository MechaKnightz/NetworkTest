using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Messenger;
using Library.Tiles;
using Lidgren.Network;
using MapMaker.Tiles;

namespace Library
{
    public static class DataConvert
    {
        public static ITile TileFromTileType(TileType type)
        {
            ITile tile = new Air();
            switch (type)
            {
                case TileType.Air:
                    tile = new Air();
                    break;
                case TileType.Dirt:
                    tile = new Dirt();
                    break;
                default:
                    tile = new Air();
                    break;
            }
            return tile;
        }
        public static ITile ReadTile(NetIncomingMessage inc)
        {
            var tile = TileFromTileType((TileType)inc.ReadByte());

            tile.Read(inc);

            return tile;
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
            outmsg.Write(message.Timestamp.ToString());
        }

        public static void ReadMessage(NetIncomingMessage inc, Message message)
        {
            message.Sender = inc.ReadString();
            message.Text = inc.ReadString();
            message.Timestamp = TimeSpan.Parse(inc.ReadString());
        }

        public static void WriteShot(NetOutgoingMessage outmsg, Shot shot)
        {
            outmsg.Write(shot.Speed);
            outmsg.Write(shot.X);
            outmsg.Write(shot.Y);
            outmsg.Write(shot.Damage);
            outmsg.Write(shot.Rotation);
            outmsg.Write(shot.Radius);
        }
        public static void ReadShot(NetIncomingMessage inc, Shot shot)
        {
            shot.Speed = inc.ReadFloat();
            shot.X = inc.ReadFloat();
            shot.Y = inc.ReadFloat();
            shot.Damage = inc.ReadFloat();
            shot.Rotation = inc.ReadFloat();
            shot.Radius = inc.ReadFloat();
        }

        public static void ReadRoom(NetIncomingMessage inc, GameRoom room)
        {
            room.Name = inc.ReadString();
            ReadMap(inc, room.Map);
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

        public static void ReadMap(NetIncomingMessage inc, Map map)
        {
            var rows = inc.ReadInt32();
            for (int i = 0; i < rows; i++)
            {
                var columnLength = inc.ReadInt32();
                for (int j = 0; j < columnLength; j++)
                {
                    map.MapData[i][j] = map.MapData[i][j].Read(inc);
                }
            }
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
