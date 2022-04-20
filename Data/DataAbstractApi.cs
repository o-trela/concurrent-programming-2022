using System;

namespace Data
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
