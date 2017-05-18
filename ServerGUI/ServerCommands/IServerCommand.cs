﻿using System.Collections.Generic;
using Library;
using Lidgren.Network;
using ServerGUI.ServerLogger;

namespace ServerGUI.ServerCommands
{
    public interface IServerCommand
    {
        int ParameterCount { get; set; }
        bool Run(LoggerManager loggerManager, World world, List<string> parameters, out string runMessage);
    }
}
