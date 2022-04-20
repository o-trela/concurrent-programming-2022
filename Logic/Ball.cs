namespace BallSimulator.Logic
{
    public class Ball
    {
        public int Radius { get; private set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public float Direction { get; set; } // [0, 1] where 0 and 1 is right, 0.25 is down...

        public Ball(int radius, int posX, int posY)
            : this(radius, new Vector2(posX, posY))
        { }

        public Ball(int radius, Vector2 position)
        {
            Radius = radius;
            Position = position;
        }
    }
}