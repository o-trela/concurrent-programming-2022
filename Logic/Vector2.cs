namespace BallSimulator.Logic
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 Up = new Vector2(0f, 1f);
        public static readonly Vector2 Down = new Vector2(0f, -1f);
        public static readonly Vector2 Left = new Vector2(-1f, 0f);
        public static readonly Vector2 Right = new Vector2(1f, 0f);

        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static float Distance(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(DistanceSquared(point1, point2));
        }

        public static float DistanceSquared(Vector2 point1, Vector2 point2)
        {
            float xDifference = point1.X - point2.X;
            float yDifference = point1.Y - point2.Y;
            return xDifference * xDifference + yDifference * yDifference;
        }

        public bool IsZero()
        {
            return Equals(Zero);
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2 vector
                && Equals(vector);
        }

        public bool Equals(Vector2 other)
        {
            float xDiff = X - other.X;
            float yDiff = Y - other.Y;
            return xDiff * xDiff + yDiff * yDiff < 9.99999944E-11f;
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X + rhs.X,
                Y = lhs.Y + rhs.Y,
            };
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X - rhs.X,
                Y = lhs.Y - rhs.Y,
            };
        }

        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X * rhs.X,
                Y = lhs.Y * rhs.Y,
            };
        }

        public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2
            {
                X = lhs.X / rhs.X,
                Y = lhs.Y / rhs.Y,
            };
        }
        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2
            {
                X = -vector.X,
                Y = -vector.Y,
            };
        }

        public static Vector2 operator *(Vector2 lhs, float d)
        {
            return new Vector2
            {
                X = lhs.X * d,
                Y = lhs.Y * d,
            };
        }

        public static Vector2 operator /(Vector2 lhs, float d)
        {
            return new Vector2
            {
                X = lhs.X / d,
                Y = lhs.Y / d,
            };
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }
    }
}
