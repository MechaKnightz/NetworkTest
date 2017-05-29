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
        Move,
        StartState,
        AllShots,
        PlayerHealth
    }
}
