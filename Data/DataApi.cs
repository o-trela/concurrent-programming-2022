namespace BallSimulator.Data
{
    internal class DataApi : DataAbstractApi
    {
        public override int BoardHeight { get; } = 450;
        public override int BoardWidth { get; } = 650;
        public override int BallDiameter { get; } = 20;
    }
}
