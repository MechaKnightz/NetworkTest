using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Lidgren.Network;
using MongoDB.Driver;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class MouseInputCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player,
            List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            var button = (MouseButton)inc.ReadByte();

            var x = inc.ReadFloat();
            var y = inc.ReadFloat();

            player = Server.GetPlayer(inc, allPlayers);

            var room = Server.GetGameRoom(player, gameRooms);

            switch (button)
            {
                case MouseButton.Left:
                    InputHandler.LeftClick(player, room.Map, x, y);
                    break;
                case MouseButton.Right:
                    InputHandler.RightClick(allPlayers, player, room.Map, x, y);
                    break;
                case MouseButton.Middle:
                    InputHandler.MiddleClick(player, room.Map, x, y);
                    break;
                case MouseButton.Mouse4:
                    break;
                case MouseButton.Mouse5:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
