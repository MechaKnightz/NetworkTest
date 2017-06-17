using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Xna.Framework;
using ServerGUI.ServerLogger;

namespace ServerGUI.ServerCommands
{
    public class TeleportCommand : IServerCommand
    {
        public CommandType Type { get; set; } = CommandType.Parameters;
        public string CommandName { get; set; } = "teleport";
        public int ParameterCount { get; set; } = 2;

        public bool Run(LoggerManager loggerManager, List<Player> allPlayers, List<string> parameters, out string runMessage)
        {
            var firstPlayer = allPlayers.FirstOrDefault(x => x.Username == parameters[0]);
            var secondPlayer = allPlayers.FirstOrDefault(x => x.Username == parameters[1]);

            //todo
            runMessage = firstPlayer.Username + " teleported to " + secondPlayer.Username + " at pos: " +
                         new Vector2(secondPlayer.X, secondPlayer.Y);
            return true;
        }
    }
}
