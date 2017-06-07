using System.Collections.Generic;
using System.Linq;
using Library;
using Lidgren.Network;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class SendRoomStartState : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player,
            List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            var outmsg = server.CreateMessage();

            outmsg.Write((byte)PacketTypes.RoomStartState);

            DataConvert.WriteRoom(outmsg, GetGameRoom(GetPlayerFromConnection(inc, allPlayers), gameRooms));

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }


        private static GameRoom GetGameRoom(Player player, List<GameRoom> gameRooms)
        {
            for (int i = 0; i < gameRooms.Count; i++)
            {
                if (gameRooms[i].Players.Any(x => x.Username == player.Username))
                {
                    return gameRooms[i];
                }
            }
            return null;
        }

        private static Player GetPlayerFromConnection(NetIncomingMessage inc, List<Player> allPlayers)
        {
            return allPlayers.FirstOrDefault(x => x.Conn == inc.SenderConnection);
        }
    }
}
