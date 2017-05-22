using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Shot
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Rotation { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }
        public float Radius { get; set; }

        public string ParentName { get; set; }
        public float Duration { get; set; }

        public DateTime CreatedTime { get; set; }

        public Shot() { }

        public Shot(float x, float y, float rotation, float speed, float damage, float radius, float duration, string parentName)
        {
            X = x;
            Y = y;
            Rotation = rotation;
            Speed = speed;
            Damage = damage;
            Radius = radius;
            ParentName = parentName;
            Duration = duration;

            CreatedTime = DateTime.Now;
        }
    }
}
