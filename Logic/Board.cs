namespace BallSimulator.Logic;
public class Board
{
    public int Height { get; init; }
    public int Width { get; init; }
    public Vector2 YBoundry => new(0, Height);
    public Vector2 XBoundry => new(0, Width);

    public Board(int height, int width)
    {
        Height = height;
        Width = width;
    }
}
