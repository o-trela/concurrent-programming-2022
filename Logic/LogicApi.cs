using System;
using System.Collections.Generic;
using System.Text;
using BallSimulator.Data;

namespace BallSimulator.Logic
{
    public class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _data;

        public LogicApi(DataAbstractApi data) 
        {
            _data = data;
        }
    }
}
