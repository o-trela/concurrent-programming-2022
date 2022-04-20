using System;
using System.Collections.Generic;
using System.Text;

using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class BallModel
    {
        private readonly Ball _ball;

        public int Radius => _ball.Radius;
        public int PosX => _ball.PosX;
        public int PosY => _ball.PosY;
        public float Speed => _ball.Speed;
        public float Direction => _ball.Direction;

        public BallModel(Ball ball)
        {
            _ball = ball;
        }
    }
}
