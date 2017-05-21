using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class Shot
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Rotation { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }

        public Shot() { }

        public Shot(float x, float y, float rotation, float speed, float damage)
        {
            X = x;
            Y = y;
            Rotation = rotation;
            Speed = speed;
            Damage = damage;
        }
    }
}
