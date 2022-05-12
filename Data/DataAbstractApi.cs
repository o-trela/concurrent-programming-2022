namespace BallSimulator.Data;

public abstract class DataAbstractApi
{
    public abstract int BoardHeight { get; }
    public abstract int BoardWidth { get; }
    public abstract float MaxSpeed { get; }
    public abstract int MinDiameter { get; }
    public abstract int MaxDiameter { get; }

    public static DataAbstractApi CreateDataApi()
    {
        return new DataApi();
    }
}
