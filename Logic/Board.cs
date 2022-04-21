namespace BallSimulator.Logic
{
    public class Board
    {
        public int Height { get; }
        public int Width { get; }
        public Vector2 yBoundry => new Vector2(0, Height);
        public Vector2 xBoundry => new Vector2(0, Width);

        public Board(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
