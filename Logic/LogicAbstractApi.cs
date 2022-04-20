using System;
using System.Collections.Generic;
using System.Text;
using BallSimulator.Data;

namespace BallSimulator.Logic
{
    public abstract class LogicAbstractApi
    {
        public static LogicAbstractApi CreateLogicApi(DataAbstractApi data = default)
        {
            return new LogicApi(data ?? DataAbstractApi.CreateDataApi());
        }
    }
}
