namespace BallSimulator.Data
{
    public abstract class DataAbstractApi
    {
        public abstract int BoardHeight { get; }
        public abstract int BoardWidth { get; }
        public abstract int BallDiameter { get; }

        public static DataAbstractApi CreateDataApi()
        {
            return new DataApi();
        }
    }
}
