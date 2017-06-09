using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Library
{
    public class GameRoom
    {
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public Map Map { get; set; }

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
            player.Velocity += player.Gravity;

            if (!InputHandler.MoveWithAdjust(new Vector2(0, player.Velocity), player, Map) && player.Velocity > 0)
            {
                player.Velocity = 0;
                player.OnGround = true;
                player.IsJumping = false;
            }
            else if(!InputHandler.MoveWithAdjust(new Vector2(0, player.Velocity), player, Map) && player.Velocity < 0)
            {
                player.Velocity = 0;
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
