namespace BallSimulator.Data
{
    internal class DataApi : DataAbstractApi
    {
        public override int BoardHeight { get; } = 440;
        public override int BoardWidth { get; } = 640;
        public override int BallDiameter { get; } = 10;
    }
}
