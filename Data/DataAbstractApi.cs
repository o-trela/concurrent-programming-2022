using System;

namespace BallSimulator.Data
{
    public abstract class DataAbstractApi
    {
        public static DataAbstractApi CreateDataApi()
        {
            return new DataApi();
        }
    }
}
