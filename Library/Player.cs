using System;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.ComponentModel;

namespace Library
{
    public class Player : ICloneable, INotifyPropertyChanged
    {
        public Player() { }
        public Player(string username, Vector2 position, float health, float rotation, float speed, float radius, NetConnection conn = null)
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

        public const float Width = Map.TileSize * 1.8f;
        public const float Height = Map.TileSize * 2.8f;
        public const float JumpStartVelocity = 12.0f;
        public const float CancelJumpVelocity = 6.0f;

        public bool IsDirty;
        public float Speed;
        public float Rotation;
        public float Health;
        public float Radius;
        public NetConnection Conn;
        public int LatestInput;
        public TimeSpan JumpDuration = TimeSpan.FromSeconds(2);
        public DateTime JumpTime;

        public bool OnGround = false;
        public bool IsJumping = false;
        public float Velocity;
        public readonly float Gravity = 0.5f;

        private float _x;
        public float X
        {
            get { return _x; }
            set
            {
                _x = value;
                IsDirty = true;
            }
        }
        private float _y;
        public float Y
        {
            get { return _y; }
            set
            {
                _y = value;
                IsDirty = true;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
