﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Library.Messenger;

namespace Library
{
    public class GameRoom
    {
        public string Name { get; set; }
        public List<Player> Players { get; }
        public Map Map { get; set; }

        //public List<Message> ChatMessages { get; set; }

        public GameRoom(string name)
        {
            Name = name;
            Players = new List<Player>();
            Map = new Map(MapSize.Medium);
        }

        public GameRoom()
        {
            Players = new List<Player>();
            Map = new Map(MapSize.Medium);
        }

        private void GravityMove(Player player)
        {
            InputHandler.MoveWithAdjust(new Vector2(player.VelocityX, 0), player, Map);

            if (player.VelocityX >= player.GravityX) player.VelocityX -= player.GravityX;
            else if(player.VelocityX > 0) player.VelocityX = 0;
            else if(player.VelocityX <= -player.GravityX) player.VelocityX += player.GravityX;
            else if (player.VelocityX < 0) player.VelocityX = 0;
            //player.VelocityX = 0;


            player.VelocityY += player.Gravity;

            if (!InputHandler.MoveWithAdjust(new Vector2(0, player.VelocityY), player, Map) && player.VelocityY > 0)
            {
                player.VelocityY = 0;
                player.OnGround = true;
                player.IsJumping = false;
            }
            else if(!InputHandler.MoveWithAdjust(new Vector2(0, player.VelocityY), player, Map) && player.VelocityY < 0)
            {
                player.VelocityY = 0;
            }
            else player.OnGround = false;
        }

        public void HandleInput(Player player, Keys key)
        {
            InputHandler.MovePlayer(player, Map, key);
        }

        public static void GravityMovePlayers(List<GameRoom> gameRooms)
        {
            for (int i = 0; i < gameRooms.Count; i++)
            {
                for (int j = 0; j < gameRooms[i].Players.Count; j++)
                {
                    gameRooms[i].GravityMove(gameRooms[i].Players[j]);
                }
            }
        }
    }
}
