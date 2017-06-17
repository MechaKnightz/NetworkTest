using System;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.ComponentModel;

namespace Library
{
    public class Player : ICloneable, INotifyPropertyChanged
    {
        public Player() { }
        public Player(string username, Vector2 position, float health, float speed, NetConnection conn = null)
        {
            Username = username;
            X = position.X;
            Y = position.Y;
            Health = health;
            Speed = speed;
            Conn = conn;
        }

        public const float Width = Map.TileSize * 1.8f;
        public const float Height = Map.TileSize * 2.8f;
        public const float JumpStartVelocity = 12.0f;
        public const float CancelJumpVelocity = 6.0f;

        public bool IsDirty;
        public float Speed;
        public float Health;
        public readonly NetConnection Conn;
        public int LatestInput;
        public TimeSpan JumpDuration = TimeSpan.FromSeconds(2);
        public DateTime JumpTime;

        public float VelocityX;
        public float GravityX = 2f;

        public bool OnGround = false;
        public bool IsJumping = false;
        public float VelocityY;
        public float Gravity = 0.5f;
        public float Range = 500;

        public DateTime LastMessageTime = DateTime.MinValue;

        public Inventory Inventory { get; set; }

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

        public override string ToString()
        {
            return Username;
        }
    }
}
