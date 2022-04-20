using System;

namespace BallSimulator.Logic
{
    public class Ball
    {
        public int Radius { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }

        public Ball(int radius, int posX, int posY)
            : this(radius, new Vector2(posX, posY))
        { }

        public Ball(int radius, Vector2 position)
        {
            Radius = radius;
            Position = position;
        }

        public void Move(float strength)
        {
            if (!strength.Between(0f, 1f)) throw new ArgumentException("Strenght must be between 0 and 1!", nameof(strength));
            Position += Speed * strength;
        }
    }
}