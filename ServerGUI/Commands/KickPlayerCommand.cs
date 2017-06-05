using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    class KickPlayerCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoCLient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            for (int i = 0; i < allPlayers.Count; i++)
            {
                if(player.Username == allPlayers[i].Username) allPlayers.RemoveAt(i);
            }
            for (int i = 0; i < gameRooms.Count; i++)
            {
                for (int j = 0; j < gameRooms[i].Players.Count; j++)
                {
                    if (gameRooms[i].Players[j].Username == player.Username)
                    {
                        gameRooms[i].Players.RemoveAt(j);

                        for (int k = 0; k < gameRooms[i].Players.Count; k++)
                        {
                            var outmsg = server.CreateMessage();

                            outmsg.Write((byte)PacketTypes.PlayerLeave);

                            outmsg.Write(gameRooms[i].Players[j].Username);

                            server.SendMessage(outmsg, gameRooms[i].Players[k].Conn, NetDeliveryMethod.ReliableOrdered);
                        }
                    }
                }
            }
            loggerManager.ServerMsg(player.Username + " has been removed from the server.");
        }
    }
}
