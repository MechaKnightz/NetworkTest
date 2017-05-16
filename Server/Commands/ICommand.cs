using Library;
using Lidgren.Network;

namespace Server.Commands
{
    public interface ICommand
    {
        void Run(NetServer server, NetIncomingMessage inc,Player player, World world);
    }
}
