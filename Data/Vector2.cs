namespace BallSimulator.Data;

public readonly struct Vector2 : IEquatable<Vector2>
{
    public static readonly Vector2 Zero = new(0f, 0f);
    public static readonly Vector2 One = new(1f, 1f);
    public static readonly Vector2 Up = new(0f, -1f);
    public static readonly Vector2 Down = new(0f, 1f);
    public static readonly Vector2 Left = new(-1f, 0f);
    public static readonly Vector2 Right = new(1f, 0f);

    public float X { readonly get; init; }
    public float Y { readonly get; init; }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static float Distance(in Vector2 point1, in Vector2 point2)
    {
        return MathF.Sqrt(DistanceSquared(point1, point2));
    }

    public static float DistanceSquared(in Vector2 point1, in Vector2 point2)
    {
        float xDifference = point1.X - point2.X;
        float yDifference = point1.Y - point2.Y;
        return xDifference * xDifference + yDifference * yDifference;
    }

    public static float Scalar(in Vector2 point1, in Vector2 point2)
    {
        return point1.X * point2.X + point1.Y * point2.Y;
    }

    public bool IsZero()
    {
        return Equals(Zero);
    }

    public static Vector2 operator +(in Vector2 lhs, in Vector2 rhs)
    {
        return new Vector2
        {
            X = lhs.X + rhs.X,
            Y = lhs.Y + rhs.Y,
        };
    }

    public static Vector2 operator -(in Vector2 lhs, in Vector2 rhs)
    {
        return new Vector2
        {
            X = lhs.X - rhs.X,
            Y = lhs.Y - rhs.Y,
        };
    }

    public static Vector2 operator *(in Vector2 lhs, in Vector2 rhs)
    {
        return new Vector2
        {
            X = lhs.X * rhs.X,
            Y = lhs.Y * rhs.Y,
        };
    }

    public static Vector2 operator /(in Vector2 lhs, in Vector2 rhs)
    {
        return new Vector2
        {
            X = lhs.X / rhs.X,
            Y = lhs.Y / rhs.Y,
        };
    }
    public static Vector2 operator -(in Vector2 vector)
    {
        return new Vector2
        {
            X = -vector.X,
            Y = -vector.Y,
        };
    }

    public static Vector2 operator *(in Vector2 lhs, float d)
    {
        return new Vector2
        {
            X = lhs.X * d,
            Y = lhs.Y * d,
        };
    }

    public static Vector2 operator /(in Vector2 lhs, float d)
    {
        return new Vector2
        {
            X = lhs.X / d,
            Y = lhs.Y / d,
        };
    }

    public static bool operator ==(in Vector2 lhs, in Vector2 rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(in Vector2 lhs, in Vector2 rhs)
    {
        return !(lhs == rhs);
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
        return HashCode.Combine(X, Y);
    }
}
