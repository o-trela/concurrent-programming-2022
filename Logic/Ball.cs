using System;

namespace BallSimulator.Logic
{
    public class Ball : IEquatable<Ball>
    {
        public int Diameter { get; }
        public int Radius { get; }
        public Vector2 Position { get; private set; }
        public Vector2 Speed { get; private set; }

        public Ball(int diameter, int posX, int posY, float speedX, float speedY)
            : this(diameter, new Vector2(posX, posY), new Vector2(speedX, speedY))
        { }

        public Ball(int diameter, Vector2 position, Vector2 speed)
        {
            Diameter = diameter;
            Position = position;
            Speed = speed;
            Radius = diameter / 2;
        }

        public void Move(Vector2 xBoundry, Vector2 yBoundry, float strength = 1f)
        {
            if (!strength.Between(0f, 1f)) throw new ArgumentException("Strenght must be between 0 and 1!", nameof(strength));

            if (Speed.IsZero()) return;

            Position += Speed * strength;

            var (posX, posY) = Position;
            if (!posX.Between(xBoundry.X, xBoundry.Y, Radius))
            {
                Speed = new Vector2(-Speed.X, Speed.Y);
            }
            if (!posY.Between(yBoundry.X, yBoundry.Y, Radius))
            {
                Speed = new Vector2(Speed.X, -Speed.Y);
            }
        }

        public Vector2 AddSpeed(Vector2 speed)
        {
            Speed += speed;
            return Speed;
        }

        public override bool Equals(object obj)
        {
            return obj is Ball ball
                && Equals(ball);
        }

        public bool Equals(Ball other)
        {
            return other is Ball
                && Diameter == other.Diameter
                && Position == other.Position
                && Speed == other.Speed;
        }

        public override int GetHashCode()
        {
            int hashCode = 883467613;
            hashCode = hashCode * -1521134295 + Diameter.GetHashCode();
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Speed.GetHashCode();
            return hashCode;
        }
    }
}