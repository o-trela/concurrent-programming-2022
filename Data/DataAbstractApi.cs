using System;

namespace BallSimulator.Data
{
    public abstract class DataAbstractApi
    {
        public abstract int Radius { get; }

        public static DataAbstractApi CreateDataApi()
        {
            return new DataApi();
        }
    }
}
