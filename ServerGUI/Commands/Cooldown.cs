using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace ServerGUI.Commands
{
    public class Cooldown
    {
        public NetConnection SenderConnection { get; set; }
        public DateTime CreatedTime { get; set; }

        public Cooldown(NetConnection senderConnection)
        {
            SenderConnection = senderConnection;
            CreatedTime = DateTime.Now;
        }
    }
}
