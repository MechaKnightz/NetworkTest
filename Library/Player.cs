using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Library
{
    public class Player
    {
        public Player() { }
        public Player(string name, Vector2 position, float health, float rotation, float speed, NetConnection conn)
        {
            Name = name;
            X = position.X;
            Y = position.Y;
            Health = health;
            Rotation = rotation;
            Speed = speed;
            Conn = conn;
        }

        public float Speed;
        public float Rotation;
        public float Health;
        public string Name;
        public float X;
        public float Y;
        public NetConnection Conn;
    }
}
