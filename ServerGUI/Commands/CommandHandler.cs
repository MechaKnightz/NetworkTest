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
                case PacketTypes.KeyInput:
                    return new KeyInputCommand();
                case PacketTypes.StartState:
                    break;
                case PacketTypes.JoinRoom:
                    return new JoinRoomCommand();
                case PacketTypes.MouseInput:
                    return new MouseInputCommand();
                case PacketTypes.JumpCancel:
                    return new JumpCancelCommand();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
    }
}
