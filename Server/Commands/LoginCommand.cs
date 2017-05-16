using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;

namespace Server.Commands
{
    class LoginCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            throw new NotImplementedException();
        }
    }
}
