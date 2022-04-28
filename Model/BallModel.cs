using BallSimulator.Logic;
using System;

namespace BallSimulator.Presentation.Model
{
    public class BallModel
    {
        private readonly Ball _ball;

        public int Diameter => _ball.Diameter;
        public int Radius => _ball.Radius;
        public Vector2 Position => CalculateOffsetPosition(_ball.Position);
        public Vector2 Speed => _ball.Speed;


        public BallModel(Ball ball)
        {
            _ball = ball;
        }

        private Vector2 CalculateOffsetPosition(Vector2 position)
        {
            return new Vector2(position.X - Radius, position.Y - Radius);
        }
    }
}
