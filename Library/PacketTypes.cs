using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public enum PacketTypes
    {
        Login,
        Register,
        PlayerPosition,
        AllPlayerPosition,
        KeyInput,
        StartState,
        PlayerHealth,
        JoinRoom,
        RoomStartState,
        PlayerLeave,
        MouseInput,
        TileData
    }
}
