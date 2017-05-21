using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    class KickPlayerCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            loggerManager.ServerMsg(player.Username + " has been kicked from the server.");
            world.Players.Remove(player);
        }
    }
}
