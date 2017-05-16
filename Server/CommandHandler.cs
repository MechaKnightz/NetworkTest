using System;
using Library;
using Lidgren.Network;
using Server.Commands;

namespace Server
{
    public static class CommandHandler
    {
        public static ICommand GetCommand(NetIncomingMessage inc)
        {
            switch ((PacketTypes) inc.ReadByte())
            {
                case PacketTypes.Login:
                    return new LoginCommand();
                case PacketTypes.PlayerPosition:
                    break;
                case PacketTypes.AllPlayerPosition:
                    break;
                case PacketTypes.Move:
                    return new InputCommand();
                case PacketTypes.StartState:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
    }
}
