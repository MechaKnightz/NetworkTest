using System;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Library
{
    public class Player : ICloneable
    {
        public Player() { }
        public Player(string username, Vector2 position, float health, float rotation, float speed, float radius, NetConnection conn)
        {
            Username = username;
            X = position.X;
            Y = position.Y;
            Health = health;
            Rotation = rotation;
            Speed = speed;
            Radius = radius;
            Conn = conn;
        }

        public float Speed;
        public float Rotation;
        public float Health;
        public string Username;
        public float X;
        public float Y;
        public float Radius;
        public NetConnection Conn;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
