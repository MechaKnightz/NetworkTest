using System;
using Library;
using Lidgren.Network;

namespace ServerGUI.Commands
{
    public static class CommandHandler
    {
        public static ICommand GetCommand(NetIncomingMessage inc)
        {
            switch ((PacketTypes) inc.ReadByte())
            {
                case PacketTypes.PlayerPosition:
                    break;
                case PacketTypes.AllPlayerPosition:
                    break;
                case PacketTypes.Move:
                    return new InputCommand();
                case PacketTypes.StartState:
                    break;
                case PacketTypes.JoinRoom:
                    return new JoinRoomCommand();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
    }
}
