using Library;
using Lidgren.Network;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public interface ICommand
    {
        void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc,Player player, World world);
    }
}
