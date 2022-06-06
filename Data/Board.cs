namespace BallSimulator.Data;

public class Board
{
    public int Height { get; init; }
    public int Width { get; init; }
    public Vector2 BoundryY => new(0, Height);
    public Vector2 BoundryX => new(0, Width);

    public Board(int height, int width)
    {
        Height = height;
        Width = width;
    }
}
