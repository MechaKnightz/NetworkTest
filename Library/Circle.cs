using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Library
{
    public struct Circle
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Circle && Equals((Circle) obj);
        }

        public float X { get; set; } 
        public float Y { get; set; }
        public float Radius { get; set; }

        public static Circle Empty { get; set; } = new Circle(0, new Vector2(0, 0));
        public static Circle UnitCircle { get; set; } = new Circle(1, new Vector2(0, 0));

        public Circle(float radius, float x, float y)
        {
            Radius = radius;
            X = x;
            Y = y;
        }
        public Circle(float radius, Vector2 position)
        {
            Radius = radius;
            X = position.X;
            Y = position.Y;
        }

        public Circle(float radius)
        {
            Radius = radius;
            X = 0;
            Y = 0;
        }

        public bool Intersect(Circle other)
        {
            var distance = Vector2.Distance(new Vector2(X, Y), new Vector2(other.X, other.Y));
            return distance < Radius + other.Radius;
        }

        public bool Intersect(Rectangle other)
        {
            var corners = new List<Vector2>
            {
                new Vector2(other.Left, other.Top),
                new Vector2(other.Right, other.Top),
                new Vector2(other.Right, other.Bottom),
                new Vector2(other.Left, other.Bottom)
            };

            var temp = this;
            return corners.Any(corner => Vector2.Distance(new Vector2(temp.X, temp.Y), corner) < temp.Radius);
        }

        public Vector2 GetSide(Side side)
        {
            switch (side)
            {
                case Side.Right:
                    return new Vector2(X + Radius, 0);
                case Side.Upper:
                    return new Vector2(0, Y + Radius);
                case Side.Left:
                    return new Vector2(X -Radius, 0);
                case Side.Lower:
                    return new Vector2(0, Y -Radius);
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public bool Contains(Vector2 position)
        {
            return Vector2.Distance(new Vector2(X, Y), position) < Radius;
        }
        public bool Contains(Circle other)
        {
            if (other.Radius > Radius) return false;
            return !(Vector2.Distance(new Vector2(X, Y), new Vector2(other.X, other.Y)) + other.Radius > Radius);
        }

        public bool Contains(Rectangle other)
        {
            var corners = new List<Vector2>
            {
                new Vector2(other.Left, other.Top),
                new Vector2(other.Right, other.Top),
                new Vector2(other.Right, other.Bottom),
                new Vector2(other.Left, other.Bottom)
            };

            var temp = this;
            return corners.All(corner => !(Vector2.Distance(new Vector2(temp.X, temp.Y), corner) > temp.Radius));
        }

        public bool Equals(Circle circle)
        {
            return circle.Equals(this);
        }
        public void Offset(Vector2 position)
        {
            X += position.X;
            Y += position.Y;
        }
        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }
        public static bool operator ==(Circle circle, Circle other)
        {
            return circle.Equals(other);
        }
        public static bool operator !=(Circle circle, Circle other)
        {
            return !(circle == other);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public enum Side
        {
            Right,
            Upper,
            Left,
            Lower
        }
    }
}
