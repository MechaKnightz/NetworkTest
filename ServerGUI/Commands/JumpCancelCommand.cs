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
    public class JumpCancelCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, MongoClient mongoClient, NetServer server, NetIncomingMessage inc, Player player, List<Player> allPlayers, List<GameRoom> gameRooms)
        {
            player = Server.GetPlayer(inc, allPlayers);
            //Todo make it so you can't cheat by canceling jump mid-air
            //register last jump send tomman maybe
            if (player.VelocityY < -Player.CancelJumpVelocity && !player.OnGround && player.IsJumping)
            {
                player.VelocityY = -Player.CancelJumpVelocity;
                player.IsJumping = false;
            }
        }
    }
}
