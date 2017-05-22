using System;
using Library;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using ServerGUI.ServerLogger;

namespace ServerGUI.Commands
{
    public class ShootCommand : ICommand
    {
        public void Run(LoggerManager loggerManager, NetServer server, NetIncomingMessage inc, Player player, World world)
        {
            if (player.ShotLast + TimeSpan.FromSeconds(player.Cooldown) > DateTime.Now)
                return;
            player.ShotLast = DateTime.Now;
            var shot = new Shot(player.X, player.Y, player.Rotation, 4, 1, 20, 3, player.Username);
            loggerManager.ServerMsg(player.Username + " shot at: " + new Vector2(shot.X, shot.Y));
            world.Shots.Add(shot);
        }
    }
}