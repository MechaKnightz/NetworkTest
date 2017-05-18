using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using ServerGUI.ServerLogger;

namespace ServerGUI.ServerCommands
{
    public class ServerCommandHandler
    {
        private List<IServerCommand> Commands { get; set; }= new List<IServerCommand>();
        private readonly LoggerManager LoggerManager;
        private readonly World World;

        public ServerCommandHandler(LoggerManager loggerManager, World world)
        {
            World = world;
            LoggerManager = loggerManager;
            //Commands
            Commands.Add(new TeleportCommand());
        }

        public bool HandleCommandString(string commandString, out string runMessage)
        {
            string commandName;
            try
            {
                commandName = commandString.Substring(0, commandString.IndexOf(" "));
            }
            catch (Exception e)
            {
                runMessage = "Invalid command format\nException: " + e.Message;
                throw;
                return false;
            }
            foreach (var serverCommand in Commands)
            {
                if (commandName != serverCommand.CommandName) continue;

                var returnBool = HandleCommand(commandString, out runMessage, serverCommand);
                return returnBool;
            }

            runMessage = "";
            return false;
        }

        private bool HandleCommand(string fullString, out string runMessage, IServerCommand command)
        {
            bool returnBool;
            switch (command.Type)
            {
                case CommandType.NoParameters:
                    returnBool = command.Run(LoggerManager, World, null, out runMessage);
                    break;
                case CommandType.Parameters:
                    //command is CommandName "param" "param"
                    fullString = CommandName(fullString, command.CommandName + " ");
                    //command is now "param" "param" et.c
                    var parameters= new List<string>();
                    fullString.Insert(fullString.Length, " ");
                    int lastPos = 0;
                    for (int i = 0; i < command.ParameterCount; i++)
                    {
                        parameters.Add(fullString.Substring(lastPos, fullString.IndexOf(" ")));
                        lastPos = parameters[i].Length;
                    }
                    returnBool = command.Run(LoggerManager, World, parameters, out runMessage);
                    return returnBool;
                case CommandType.Bool:
                    //command is CommandName "param" "param"
                    fullString = CommandName(fullString, command.CommandName + " ");
                    //command is now "param" "param" et.c
                    var parameters2 = new List<string>();
                    fullString.Insert(fullString.Length, " ");
                    parameters2.Add(fullString.Substring(0, fullString.IndexOf(" ")));

                    returnBool = command.Run(LoggerManager, World, parameters2, out runMessage);
                    return returnBool;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            runMessage = "";
            return false;
        }
        public static string CommandName(string source, string remove)
        {
            int index = source.IndexOf(remove);
            return (index < 0)
                ? source
                : source.Remove(index, remove.Length);
        }
    }
}
