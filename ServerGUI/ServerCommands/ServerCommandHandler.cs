using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerGUI.ServerCommands
{
    public static class ServerCommandHandler
    {
        private static List<IServerCommand> Commands { get; set; }= new List<IServerCommand>();

        public static void Initialize()
        {
            
        }

        public static bool HandleCommandString(string commandString, out string runMessage)
        {
            foreach (var serverCommand in Commands)
            {
                //TODO
            }

            runMessage = "";
            return false;
        }
    }
}
