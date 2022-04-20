using System;
using System.Collections.Generic;
using System.Text;

namespace BallSimulator.Logic
{
    public abstract class LogicAbstractApi
    {
        public static LogicAbstractApi CreateApi()
        {
            return new BallManager(100, 200, 2);
        }
    }
}
