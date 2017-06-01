﻿using System;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.ComponentModel;

namespace Library
{
    public class Player : ICloneable, INotifyPropertyChanged
    {
        public Player() { }
        public Player(string username, Vector2 position, float health, float rotation, float speed, float radius, float cooldown, NetConnection conn = null)
        {
            Username = username;
            X = position.X;
            Y = position.Y;
            Health = health;
            Rotation = rotation;
            Speed = speed;
            Radius = radius;
            Cooldown = cooldown;
            Conn = conn;
            Falling = true;
        }

        public const float Width = Map.TileSize * 2;
        public const float Height = Map.TileSize * 3;

        public bool IsDirty;
        public bool Falling;
        public float Speed;
        public float Rotation;
        public float Health;
        public float Radius;
        public float Cooldown;
        public NetConnection Conn;

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
