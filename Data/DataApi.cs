namespace BallSimulator.Data;

internal class DataApi : DataAbstractApi
{
    public override int BoardHeight => 450;
    public override int BoardWidth => 650;
    public override int BallDiameter => 20;
    public override float MaxSpeed => 50f;
    public override int MinDiameter => 20;
    public override int MaxDiameter => 50;
}
