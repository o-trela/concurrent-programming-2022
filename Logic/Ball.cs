namespace BallSimulator.Logic
{
    public class Ball
    {
        public int Radius { get; private set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public float Speed { get; set; }
        public float Direction { get; set; } // [0, 1] where 0 and 1 is right, 0.25 is down...

        public Ball(int radius, int posX, int posY)
        {
            Radius = radius;
            PosX = posX;
            PosY = posY;
        }
    }
}