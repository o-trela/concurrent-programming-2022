namespace BallSimulator.Logic
{
    public class Board
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Board(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
