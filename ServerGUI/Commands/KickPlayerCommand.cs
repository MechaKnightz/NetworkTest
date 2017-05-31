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
            loggerManager.ServerMsg(player.Username + " has been kicked from the server.");
            allPlayers.Remove(player);
            for (int i = 0; i < gameRooms.Count; i++)
            {
                var tempPlayer = gameRooms[i].Players.FirstOrDefault(x => x.Username == player.Username);
                if (tempPlayer != null)
                {
                    gameRooms[i].Players.Remove(tempPlayer);
                }
            }
        }
    }
}
