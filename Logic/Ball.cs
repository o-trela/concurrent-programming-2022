namespace Logic;

public class Ball
{
    public int Radius { get; init; }
    public int PosX { get; set; }
    public int PosY { get; set; }

    public Ball(int radius, int posX, int posY)
    {
        Radius = radius;
        PosX = posX;
        PosY = posY;
    }
}
