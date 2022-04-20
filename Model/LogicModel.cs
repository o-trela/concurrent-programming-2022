using System;
using System.Collections.Generic;
using System.Text;

using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class LogicModel
    {
        private readonly LogicAbstractApi _logic;

        public LogicModel(LogicAbstractApi logic = null)
        {
            _logic = logic ?? LogicAbstractApi.CreateApi();
        }
    }
}
