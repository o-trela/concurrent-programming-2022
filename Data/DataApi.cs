namespace BallSimulator.Data
{
    internal class DataApi : DataAbstractApi
    {
        public override int BoardHeight { get; } = 430;
        public override int BoardWidth { get; } = 630;
        public override int BallRadius { get; } = 10;
    }
}
